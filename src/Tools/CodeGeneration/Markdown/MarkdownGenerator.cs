﻿// Copyright (c) Josef Pihrt and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using DotMarkdown;
using DotMarkdown.Linq;
using Roslynator.Metadata;
using static System.Environment;
using static DotMarkdown.Linq.MFactory;

namespace Roslynator.CodeGeneration.Markdown;

public static class MarkdownGenerator
{
    private static void AddFootnote(this MDocument document)
    {
        document.Add(NewLine, Italic("(Generated with ", Link("DotMarkdown", "http://github.com/JosefPihrt/DotMarkdown"), ")"));
    }

    public static string CreateReadMe(IEnumerable<AnalyzerMetadata> analyzers, IEnumerable<RefactoringMetadata> refactorings, IComparer<string> comparer)
    {
        MDocument document = Document(
            Heading3("List of Analyzers"),
            analyzers
                .OrderBy(f => f.Id, comparer)
                .Select(analyzer => BulletItem(analyzer.Id, " - ", Link(analyzer.Title.TrimEnd('.'), $"docs/analyzers/{analyzer.Id}.md"))),
            Heading3("List of Refactorings"),
            refactorings
                .OrderBy(f => f.Title, comparer)
                .Select(refactoring => BulletItem(Link(refactoring.Title.TrimEnd('.'), $"docs/refactorings/{refactoring.Id}.md"))));

        document.AddFootnote();

        return File.ReadAllText(@"..\text\ReadMe.txt", Encoding.UTF8)
            + NewLine
            + document;
    }

    public static string CreateRefactoringsMarkdown(IEnumerable<RefactoringMetadata> refactorings, IComparer<string> comparer)
    {
        MDocument document = Document(
            Heading2("Roslynator Refactorings"),
            GetRefactorings());

        document.AddFootnote();

        return document.ToString();

        IEnumerable<object> GetRefactorings()
        {
            foreach (RefactoringMetadata refactoring in refactorings.OrderBy(f => f.Title, comparer))
            {
                yield return Heading4($"{refactoring.Title} ({refactoring.Id})");
                yield return BulletItem(Bold("Syntax"), ": ", string.Join(", ", refactoring.Syntaxes.Select(f => f.Name)));

                if (!string.IsNullOrEmpty(refactoring.Span))
                    yield return BulletItem(Bold("Span"), ": ", refactoring.Span);

                foreach (object item in GetRefactoringSamples(refactoring))
                    yield return item;
            }
        }
    }

    private static IEnumerable<object> GetRefactoringSamples(RefactoringMetadata refactoring)
    {
        if (refactoring.Samples.Count > 0)
        {
            foreach (MElement element in GetSamples(refactoring.Samples, Heading4("Before"), Heading4("After")))
                yield return element;
        }
        else if (refactoring.Images.Count > 0)
        {
            var isFirst = true;

            foreach (ImageMetadata image in refactoring.Images)
            {
                if (!isFirst)
                    yield return NewLine;

                yield return RefactoringImage(refactoring, image.Name);
                yield return NewLine;

                isFirst = false;
            }

            yield return NewLine;
        }
        else
        {
            yield return RefactoringImage(refactoring, refactoring.Identifier);
            yield return NewLine;
            yield return NewLine;
        }
    }

    private static IEnumerable<MElement> GetSamples(
        IEnumerable<SampleMetadata> samples,
        MHeading beforeHeading,
        MHeading afterHeading)
    {
        using (IEnumerator<SampleMetadata> en = samples.GetEnumerator())
        {
            if (en.MoveNext())
            {
                while (true)
                {
                    yield return beforeHeading;
                    yield return FencedCodeBlock(en.Current.Before, LanguageIdentifiers.CSharp);

                    if (!string.IsNullOrEmpty(en.Current.After))
                    {
                        yield return afterHeading;
                        yield return FencedCodeBlock(en.Current.After, LanguageIdentifiers.CSharp);
                    }

                    if (en.MoveNext())
                    {
                        yield return HorizontalRule();
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }

    private static IEnumerable<MElement> GetAnalyzerSamples(AnalyzerMetadata analyzer)
    {
        return GetAnalyzerSamples(analyzer.Samples, analyzer.Kind);
    }

    private static IEnumerable<MElement> GetAnalyzerSamples(IReadOnlyList<SampleMetadata> samples, AnalyzerOptionKind kind)
    {
        if (samples.Count > 0)
        {
            yield return Heading2((samples.Count == 1) ? "Example" : "Examples");

            string beforeHeading = (kind == AnalyzerOptionKind.Disable)
                ? "Code"
                : "Code with Diagnostic";

            foreach (MElement item in GetSamples(samples, Heading3(beforeHeading), Heading3("Code with Fix")))
                yield return item;
        }
    }

    private static MElement CreateLink(in LinkMetadata link)
    {
        if (string.IsNullOrEmpty(link.Text))
        {
            return new MAutolink(link.Url);
        }
        else
        {
            return Link(link.Text, link.Url, link.Title);
        }
    }

    public static string CreateRefactoringMarkdown(RefactoringMetadata refactoring)
    {
        var format = new MarkdownFormat(tableOptions: MarkdownFormat.Default.TableOptions | TableOptions.FormatContent);

        MDocument document = Document(
            Heading2(refactoring.Title),
            Table(
                TableRow("Property", "Value"),
                TableRow("Id", refactoring.Id),
                TableRow("Title", refactoring.Title),
                TableRow("Syntax", string.Join(", ", refactoring.Syntaxes.Select(f => f.Name))),
                (!string.IsNullOrEmpty(refactoring.Span)) ? TableRow("Span", refactoring.Span) : null,
                TableRow("Enabled by Default", CheckboxOrHyphen(refactoring.IsEnabledByDefault))),
            CreateSummary(refactoring.Summary),
            Heading3("Usage"),
            GetRefactoringSamples(refactoring),
            CreateRemarks(refactoring.Remarks),
            CreateSeeAlso(refactoring.Links.Select(f => CreateLink(f)), Link("Full list of refactorings", "Refactorings.md")));

        document.AddFootnote();

        return document.ToString(format);
    }

    public static string CreateAnalyzerMarkdown(AnalyzerMetadata analyzer, ImmutableArray<ConfigOptionMetadata> options, IEnumerable<(string title, string url)> appliesTo = null)
    {
        IEnumerable<MInlineCode> requiredOptions = analyzer.ConfigOptions
            .Where(f => f.IsRequired)
            .Select(f => InlineCode(f.Key));

        var format = new MarkdownFormat(tableOptions: MarkdownFormat.Default.TableOptions | TableOptions.FormatContent);

        MDocument document = Document(
            Heading1($"{((analyzer.IsObsolete) ? "[deprecated] " : "")}{analyzer.Id}: {analyzer.Title.TrimEnd('.')}"),
            Table(
                TableRow("Property", "Value"),
                TableRow("Id", analyzer.Id),
                TableRow("Severity", (analyzer.IsEnabledByDefault) ? analyzer.DefaultSeverity : "None"),
                (!string.IsNullOrEmpty(analyzer.MinLanguageVersion)) ? TableRow("Minimum language version", analyzer.MinLanguageVersion) : null,
                (requiredOptions.Any()) ? TableRow("Required option", Join(" or ", requiredOptions)) : null
            ),
            CreateSummary(analyzer.Summary),
            GetAnalyzerSamples(analyzer),
            CreateOptions(analyzer, options),
            CreateRemarks(analyzer.Remarks),
            CreateAppliesTo(appliesTo));

        document.AddFootnote();

        return document.ToString(format);
    }

    public static string CreateAnalyzerOptionMarkdown(AnalyzerOptionMetadata option)
    {
        string id = option.ParentId + option.Id;

        string optionKey = option.OptionKey;

        if (!optionKey.StartsWith("roslynator."))
            optionKey = $"roslynator.{option.ParentId}." + optionKey;

        string optionValue = option.OptionValue ?? "true";

        MDocument document = Document(
            Heading1($"[deprecated] {id}: {option.Title.TrimEnd('.')}"),
            Bold($"Option {id} is obsolete, use EditorConfig option instead:"),
            FencedCodeBlock($"{optionKey} = {optionValue}"),
            GetAnalyzerSamples(option.Samples, option.Kind));

        document.AddFootnote();

        var format = new MarkdownFormat(tableOptions: MarkdownFormat.Default.TableOptions | TableOptions.FormatContent);

        return document.ToString(format);
    }

    private static IEnumerable<MElement> CreateAppliesTo(IEnumerable<(string title, string url)> appliesTo)
    {
        if (appliesTo is not null)
        {
            yield return Heading2("Applies to");
            yield return BulletList(appliesTo.Select(f => LinkOrText(f.title, f.url)));
        }
    }

    private static IEnumerable<MElement> CreateSeeAlso(params object[] content)
    {
        yield return Heading2("See Also");
        yield return BulletList(content);
    }

    public static string CreateCompilerDiagnosticMarkdown(
        CompilerDiagnosticMetadata diagnostic,
        IEnumerable<CodeFixMetadata> codeFixes,
        ImmutableArray<CodeFixOption> options,
        IComparer<string> comparer)
    {
        MDocument document = Document(
            Heading1(diagnostic.Id),
            Table(
                TableRow("Property", "Value"),
                TableRow("Id", diagnostic.Id),
                TableRow("Title", diagnostic.Title),
                TableRow("Severity", diagnostic.Severity),
                (!string.IsNullOrEmpty(diagnostic.HelpUrl)) ? TableRow("Official Documentation", Link("link", diagnostic.HelpUrl)) : null),
            Heading2("Code Fixes"),
            BulletList(codeFixes
                .Where(f => f.FixableDiagnosticIds.Any(diagnosticId => diagnosticId == diagnostic.Id))
                .Select(f => f.Title)
                .OrderBy(f => f, comparer)),
            GetOptions());

        document.AddFootnote();

        return document.ToString(MarkdownFormat.Default.WithTableOptions(MarkdownFormat.Default.TableOptions | TableOptions.FormatContent));

        IEnumerable<MElement> GetOptions()
        {
            string content = string.Join(
                NewLine,
                options
                    .Where(f => f.Key.Contains(diagnostic.Id))
                    .OrderBy(f => f.Key)
                    .Select(f => $"{f.Key} = {f.Value}"));

            if (!string.IsNullOrEmpty(content))
            {
                yield return Heading2("Options");

                yield return FencedCodeBlock(
                    content,
                    "editorconfig");
            }
        }
    }

    public static string CreateAnalyzersReadMe(IEnumerable<AnalyzerMetadata> analyzers, string title, IComparer<string> comparer)
    {
        MDocument document = Document(
            Heading2(title),
            Link("Search Analyzers", "http://pihrt.net/Roslynator/Analyzers"),
            Table(
                TableRow("Id", "Title", "Severity"),
                analyzers.OrderBy(f => f.Id, comparer).Select(f =>
                {
                    return TableRow(
                        f.Id,
                        Link(f.Title.TrimEnd('.'), $"../../docs/analyzers/{f.Id}.md"),
                        (f.IsEnabledByDefault) ? f.DefaultSeverity : "None");
                })));

        document.AddFootnote();

        return document.ToString();
    }

    public static string CreateRefactoringsReadMe(IEnumerable<RefactoringMetadata> refactorings, IComparer<string> comparer)
    {
        MDocument document = Document(
            Heading2("Roslynator Refactorings"),
            Link("Search Refactorings", "http://pihrt.net/Roslynator/Refactorings"),
            Table(
                TableRow("Id", "Title", TableColumn(HorizontalAlignment.Center, "Enabled by Default")),
                refactorings.OrderBy(f => f.Title, comparer).Select(f =>
                {
                    return TableRow(
                        f.Id,
                        Link(f.Title.TrimEnd('.'), $"../../docs/refactorings/{f.Id}.md"),
                        CheckboxOrHyphen(f.IsEnabledByDefault));
                })));

        document.AddFootnote();

        return document.ToString();
    }

    public static string CreateCodeFixesReadMe(IEnumerable<CompilerDiagnosticMetadata> diagnostics, IComparer<string> comparer)
    {
        MDocument document = Document(
            Heading2("Compiler Diagnostics Fixable with Roslynator"),
            Table(
                TableRow("Id", "Title"),
                GetRows()));

        document.AddFootnote();

        return document.ToString();

        IEnumerable<MTableRow> GetRows()
        {
            foreach (CompilerDiagnosticMetadata diagnostic in diagnostics
                .OrderBy(f => f.Id, comparer))
            {
                yield return TableRow(
                    Link(diagnostic.Id, $"../../docs/cs/{diagnostic.Id}.md"),
                    diagnostic.Title);
            }
        }
    }

    private static IEnumerable<MElement> CreateSummary(string summary)
    {
        if (!string.IsNullOrEmpty(summary))
        {
            yield return Heading2("Summary");
            yield return Raw(summary);
        }
    }

    private static IEnumerable<MElement> CreateOptions(AnalyzerMetadata analyzer, ImmutableArray<ConfigOptionMetadata> options)
    {
        IEnumerable<ConfigOptionMetadata> analyzerOptions = analyzer.ConfigOptions
            .Join(options, f => f.Key, f => f.Key, (_, g) => g)
            .OrderBy(f => f.Key);

        using (IEnumerator<ConfigOptionMetadata> en = analyzerOptions
            .GetEnumerator())
        {
            if (en.MoveNext())
            {
                yield return Heading2("Options");

                do
                {
                    string optionKey = en.Current.Key;
                    string title = en.Current.Description;
                    const string summary = null;
                    string defaultValue = en.Current.DefaultValuePlaceholder;

                    yield return Heading3(title?.TrimEnd('.'));

                    if (!string.IsNullOrEmpty(summary))
                    {
                        yield return new MText(summary);
                        yield return new MText(NewLine);
                    }

                    string helpValue = optionKey;

                    helpValue += " = ";
                    helpValue += defaultValue ?? "true";

                    yield return FencedCodeBlock(
                        helpValue,
                        "editorconfig");
                }
                while (en.MoveNext());
            }
        }
    }

    private static IEnumerable<MElement> CreateRemarks(string remarks)
    {
        if (!string.IsNullOrEmpty(remarks))
        {
            yield return Heading2("Remarks");
            yield return Raw(remarks);
        }
    }

    private static MImage RefactoringImage(RefactoringMetadata refactoring, string fileName)
    {
        return Image(refactoring.Title, $"../../images/refactorings/{fileName}.png");
    }

    private static MElement CheckboxOrHyphen(bool value)
    {
        if (value)
        {
            return CharEntity((char)0x2713);
        }
        else
        {
            return new MText("-");
        }
    }
}
