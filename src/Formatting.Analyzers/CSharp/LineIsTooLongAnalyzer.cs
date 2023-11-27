﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using Roslynator.CSharp;
using Roslynator.Formatting.CSharp;

namespace Roslynator.Formatting;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class LineIsTooLongAnalyzer : BaseDiagnosticAnalyzer
{
    private static ImmutableArray<DiagnosticDescriptor> _supportedDiagnostics;

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
    {
        get
        {
            if (_supportedDiagnostics.IsDefault)
                Immutable.InterlockedInitialize(ref _supportedDiagnostics, DiagnosticRules.LineIsTooLong);

            return _supportedDiagnostics;
        }
    }

    public override void Initialize(AnalysisContext context)
    {
        base.Initialize(context);

        context.RegisterSyntaxTreeAction(f => AnalyzeSyntaxTree(f));
    }

    private static void AnalyzeSyntaxTree(SyntaxTreeAnalysisContext context)
    {
        SyntaxTree tree = context.Tree;

        if (!tree.TryGetText(out SourceText sourceText))
            return;

        int maxLength = context.GetConfigOptions().GetMaxLineLength();

        if (maxLength <= 0)
            return;

        SyntaxNode root = tree.GetRoot(context.CancellationToken);

        int i = 0;

        SyntaxTrivia trivia = root.FindTrivia(0);

        if (trivia.SpanStart == 0
            && trivia.IsKind(SyntaxKind.SingleLineCommentTrivia, SyntaxKind.MultiLineCommentTrivia))
        {
            SyntaxTriviaList leadingTrivia = trivia.Token.LeadingTrivia;

            int count = leadingTrivia.Count;

            if (count > 1)
            {
                int j = 0;

                while (j < leadingTrivia.Count - 1
                    && leadingTrivia[j].IsKind(SyntaxKind.SingleLineCommentTrivia, SyntaxKind.MultiLineCommentTrivia)
                    && leadingTrivia[j + 1].IsKind(SyntaxKind.EndOfLineTrivia))
                {
                    i++;

                    j += 2;
                }
            }
        }

        TextLineCollection lines = sourceText.Lines;

        for (; i < lines.Count; i++)
        {
            TextLine line = lines[i];

            if (line.Span.Length <= maxLength)
                continue;

            int end = line.End;

            SyntaxToken token = root.FindToken(end);

            if (token.IsKind(SyntaxKind.StringLiteralToken))
            {
                TextSpan span = token.Span;

                if (span.End == end)
                {
                    if (span.Length >= maxLength)
                        continue;
                }
                else if (span.Contains(end)
                    && end - span.Start >= maxLength)
                {
                    continue;
                }
            }

            SyntaxTriviaList list = default;

            if (token.LeadingTrivia.Span.Contains(end))
            {
                list = token.LeadingTrivia;
            }
            else if (token.TrailingTrivia.Span.Contains(end))
            {
                list = token.TrailingTrivia;
            }

            if (list.Any())
            {
                int index = -1;

                for (int j = 0; j < list.Count; j++)
                {
                    if (list[j].Span.Contains(end))
                    {
                        trivia = list[j];
                        index = j;
                    }
                }

                if (index >= 0)
                {
                    SyntaxKind kind = trivia.Kind();

                    if (kind == SyntaxKind.MultiLineCommentTrivia
                        || kind == SyntaxKind.SingleLineDocumentationCommentTrivia
                        || kind == SyntaxKind.MultiLineDocumentationCommentTrivia)
                    {
                        continue;
                    }

                    if (kind == SyntaxKind.EndOfLineTrivia
                        && index > 0
                        && list[index - 1].IsKind(SyntaxKind.SingleLineCommentTrivia))
                    {
                        continue;
                    }
                }
            }

            DiagnosticHelpers.ReportDiagnostic(
                context,
                DiagnosticRules.LineIsTooLong,
                Location.Create(tree, line.Span),
                line.Span.Length);
        }
    }
}
