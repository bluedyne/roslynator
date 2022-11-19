﻿// Copyright (c) Josef Pihrt and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis.Text;
using Roslynator.Text;

namespace Roslynator.Testing.Text;

internal static class TextProcessor
{
    private static readonly Regex _annotatedSpanRegex = new(@"(?s)\{\|(?<identifier>[^:]+):(?<content>.*?)\|\}");

    public static (string source, ImmutableArray<TextSpan> spans) FindAnnotatedSpansAndRemove(string text, string annotationIdentifier)
    {
        (string source, ImmutableArray<(string kind, TextSpan span)> annotations) = FindAnnotatedSpansAndRemoveImpl(text, annotationIdentifier);

        return (source, ImmutableArray.CreateRange(annotations, f => f.span));
    }

    public static (string source, ImmutableArray<(string kind, TextSpan span)> annotations) FindAnnotatedSpansAndRemove(string text)
    {
        return FindAnnotatedSpansAndRemoveImpl(text);
    }

    private static (string source, ImmutableArray<(string kind, TextSpan span)> annotations) FindAnnotatedSpansAndRemoveImpl(
        string text,
        string annotationIdentifier = null)
    {
        int offset = 0;
        int lastPos = 0;

        Match match = _annotatedSpanRegex.Match(text);

        if (!match.Success)
            return (text, ImmutableArray<(string, TextSpan)>.Empty);

        StringBuilder sb = StringBuilderCache.GetInstance(text.Length);

        ImmutableArray<(string, TextSpan)>.Builder annotations = null;

        do
        {
            Group content = match.Groups["content"];

            sb.Append(text, lastPos, match.Index);
            sb.Append(content.Value);

            string identifier = match.Groups["identifier"].Value;

            if (annotationIdentifier is null
                || string.Equals(annotationIdentifier, identifier, StringComparison.Ordinal))
            {
                var span = new TextSpan(match.Index - offset, content.Length);

                (annotations ??= ImmutableArray.CreateBuilder<(string, TextSpan)>()).Add((identifier, span));
            }

            lastPos = match.Index + match.Length;
            offset += match.Length - content.Length;

            match = match.NextMatch();
        }
        while (match.Success);

        sb.Append(text, lastPos, text.Length - lastPos);

        return (
            StringBuilderCache.GetStringAndFree(sb),
            annotations?.ToImmutableArray() ?? ImmutableArray<(string, TextSpan)>.Empty);
    }

    public static (string, ImmutableArray<TextSpan>) FindSpansAndRemove(string text)
    {
        StringBuilder sb = StringBuilderCache.GetInstance(text.Length);

        var startPending = false;
        LinePositionInfo start = default;
        Stack<LinePositionInfo> stack = null;
        List<LinePositionSpanInfo> spans = null;

        int lastPos = 0;

        int line = 0;
        int column = 0;

        int length = text.Length;

        int i = 0;
        while (i < length)
        {
            switch (text[i])
            {
                case '\r':
                    {
                        if (PeekNextChar() == '\n')
                        {
                            i++;
                        }

                        line++;
                        column = 0;
                        i++;
                        continue;
                    }
                case '\n':
                    {
                        line++;
                        column = 0;
                        i++;
                        continue;
                    }
                case '[':
                    {
                        char nextChar = PeekNextChar();
                        if (nextChar == '|')
                        {
                            sb.Append(text, lastPos, i - lastPos);

                            var start2 = new LinePositionInfo(sb.Length, line, column);

                            if (stack is not null)
                            {
                                stack.Push(start2);
                            }
                            else if (!startPending)
                            {
                                start = start2;
                                startPending = true;
                            }
                            else
                            {
                                stack = new Stack<LinePositionInfo>();
                                stack.Push(start);
                                stack.Push(start2);
                                startPending = false;
                            }

                            i += 2;
                            lastPos = i;
                            continue;
                        }
                        else if (nextChar == '['
                            && PeekChar(2) == '|'
                            && PeekChar(3) == ']')
                        {
                            i++;
                            column++;
                            CloseSpan();
                            i += 3;
                            lastPos = i;
                            continue;
                        }

                        break;
                    }
                case '|':
                    {
                        if (PeekNextChar() == ']')
                        {
                            CloseSpan();
                            i += 2;
                            lastPos = i;
                            continue;
                        }

                        break;
                    }
            }

            column++;
            i++;
        }

        if (startPending
            || stack?.Count > 0)
        {
            throw new InvalidOperationException("Text span is invalid.");
        }

        sb.Append(text, lastPos, text.Length - lastPos);

        spans?.Sort(LinePositionSpanInfoComparer.Index);

        return (
            StringBuilderCache.GetStringAndFree(sb),
            spans?.Select(f => f.Span).ToImmutableArray() ?? ImmutableArray<TextSpan>.Empty);

        char PeekNextChar()
        {
            return PeekChar(1);
        }

        char PeekChar(int offset)
        {
            return (i + offset >= text.Length) ? '\0' : text[i + offset];
        }

        void CloseSpan()
        {
            if (stack is not null)
            {
                start = stack.Pop();
            }
            else if (startPending)
            {
                startPending = false;
            }
            else
            {
                throw new InvalidOperationException("Text span is invalid.");
            }

            var end = new LinePositionInfo(sb.Length + i - lastPos, line, column);

            var span = new LinePositionSpanInfo(start, end);

            (spans ??= new List<LinePositionSpanInfo>()).Add(span);

            sb.Append(text, lastPos, i - lastPos);
        }
    }

    public static (string source, string expected, ImmutableArray<TextSpan> spans) FindSpansAndReplace(
        string input,
        string replacement1,
        string replacement2 = null)
    {
        (string source, ImmutableArray<TextSpan> spans) = FindSpansAndRemove(input);

        if (spans.Length == 0)
            throw new InvalidOperationException("Text contains no span.");

        if (spans.Length > 1)
            throw new InvalidOperationException("Text contains more than one span.");

        string expected = (replacement2 is not null)
            ? source.Remove(spans[0].Start) + replacement2 + source.Substring(spans[0].End)
            : null;

        string source2 = replacement1;

        (string _, ImmutableArray<TextSpan> spans2) = FindSpansAndRemove(replacement1);

        if (spans2.Length == 0)
            source2 = "[|" + replacement1 + "|]";

        source2 = source.Remove(spans[0].Start) + source2 + source.Substring(spans[0].End);

        (string source3, ImmutableArray<TextSpan> spans3) = FindSpansAndRemove(source2);

        return (source3, expected, spans3);
    }
}
