﻿// Copyright (c) Josef Pihrt and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using Roslynator.CodeGeneration.Markdown;
using Roslynator.Metadata;
using Roslynator.Utilities;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace Roslynator.CodeGeneration;

internal static class Program
{
    private static readonly UTF8Encoding _utf8NoBom = new(encoderShouldEmitUTF8Identifier: false);

    private static async Task Main(string[] args)
    {
        if (args is null || args.Length == 0)
        {
#if DEBUG
            args = new[] { @"..\..\..\..\.." };
#else
            args = new string[] { Environment.CurrentDirectory };
#endif
        }

        string rootPath = args[0];

        StringComparer comparer = StringComparer.InvariantCulture;

        var metadata = new RoslynatorMetadata(rootPath);

        ImmutableArray<AnalyzerMetadata> analyzers = metadata.Analyzers;
        ImmutableArray<AnalyzerMetadata> codeAnalysisAnalyzers = metadata.CodeAnalysisAnalyzers;
        ImmutableArray<AnalyzerMetadata> formattingAnalyzers = metadata.FormattingAnalyzers;
        ImmutableArray<RefactoringMetadata> refactorings = metadata.Refactorings;
        ImmutableArray<CodeFixMetadata> codeFixes = metadata.CodeFixes;
        ImmutableArray<CompilerDiagnosticMetadata> compilerDiagnostics = metadata.CompilerDiagnostics;

        WriteAllText(
            @"..\docs\analyzers\README.md",
            MarkdownGenerator.CreateAnalyzersReadMe(metadata.GetAllAnalyzers().Where(f => !f.IsObsolete), "Roslynator Analyzers", comparer));
#if !DEBUG
        VisualStudioInstance instance = MSBuildLocator.QueryVisualStudioInstances().First(f => f.Version.Major == 7);

        MSBuildLocator.RegisterInstance(instance);

        using (MSBuildWorkspace workspace = MSBuildWorkspace.Create())
        {
            workspace.WorkspaceFailed += (o, e) => Console.WriteLine(e.Diagnostic.Message);

            string solutionPath = Path.Combine(rootPath, "Roslynator.sln");

            Console.WriteLine($"Loading solution '{solutionPath}'");

            Solution solution = await workspace.OpenSolutionAsync(solutionPath).ConfigureAwait(false);

            Console.WriteLine($"Finished loading solution '{solutionPath}'");

            RoslynatorInfo roslynatorInfo = await RoslynatorInfo.Create(solution).ConfigureAwait(false);

            IOrderedEnumerable<SourceFile> sourceFiles = analyzers
                .Concat(codeAnalysisAnalyzers)
                .Concat(formattingAnalyzers)
                .Select(f => new SourceFile(f.Id, roslynatorInfo.GetAnalyzerFilesAsync(f.Identifier).Result))
                .Concat(refactorings
                    .Select(f => new SourceFile(f.Id, roslynatorInfo.GetRefactoringFilesAsync(f.Identifier).Result)))
                .OrderBy(f => f.Id);

            MetadataFile.SaveSourceFiles(sourceFiles, @"..\SourceFiles.xml");
        }
#endif
        WriteAnalyzerMarkdowns(codeAnalysisAnalyzers, new (string, string)[] { ("Roslynator.CodeAnalysis.Analyzers", "https://www.nuget.org/packages/Roslynator.CodeAnalysis.Analyzers") });

        WriteAnalyzerMarkdowns(formattingAnalyzers, new (string, string)[] { ("Roslynator.Formatting.Analyzers", "https://www.nuget.org/packages/Roslynator.Formatting.Analyzers") });

        WriteAnalyzerMarkdowns(analyzers);

        DeleteInvalidAnalyzerMarkdowns();

        foreach (RefactoringMetadata refactoring in refactorings)
        {
            WriteAllText(
                $@"..\docs\refactorings\{refactoring.Id}.md",
                MarkdownGenerator.CreateRefactoringMarkdown(refactoring),
                fileMustExists: false);
        }

        IEnumerable<CompilerDiagnosticMetadata> fixableCompilerDiagnostics = compilerDiagnostics
            .Join(codeFixes.SelectMany(f => f.FixableDiagnosticIds), f => f.Id, f => f, (f, _) => f)
            .Distinct();

        ImmutableArray<CodeFixOption> codeFixOptions = typeof(CodeFixOptions).GetFields()
            .Select(f =>
            {
                var key = (string)f.GetValue(null);
                string value = f.GetCustomAttribute<CodeFixOptionAttribute>().Value;

                return new CodeFixOption(key, value);
            })
            .ToImmutableArray();

        foreach (CompilerDiagnosticMetadata diagnostic in fixableCompilerDiagnostics)
        {
            WriteAllText(
                $@"..\docs\cs\{diagnostic.Id}.md",
                MarkdownGenerator.CreateCompilerDiagnosticMarkdown(diagnostic, codeFixes, codeFixOptions, comparer),
                fileMustExists: false);
        }

        WriteAllText(
            @"..\docs\refactorings\README.md",
            MarkdownGenerator.CreateRefactoringsReadMe(refactorings.Where(f => !f.IsObsolete), comparer));

        WriteAllText(
            @"..\docs\cs\README.md",
            MarkdownGenerator.CreateCodeFixesReadMe(fixableCompilerDiagnostics, comparer));

        // find files to delete
        foreach (string path in Directory.EnumerateFiles(GetPath(@"..\docs\refactorings")))
        {
            if (!refactorings.Any(f => f.Id == Path.GetFileNameWithoutExtension(path)))
                Console.WriteLine($"FILE TO DELETE: {path}");
        }

        // find missing samples
        foreach (RefactoringMetadata refactoring in refactorings)
        {
            if (!refactoring.IsObsolete
                && refactoring.Samples.Count == 0)
            {
                foreach (ImageMetadata image in refactoring.ImagesOrDefaultImage())
                {
                    string imagePath = Path.Combine(GetPath(@"..\images\refactorings"), image.Name + ".png");

                    if (!File.Exists(imagePath))
                        Console.WriteLine($"MISSING SAMPLE: {imagePath}");
                }
            }
        }

        UpdateChangeLog();

        void WriteAnalyzerMarkdowns(IEnumerable<AnalyzerMetadata> analyzers, IEnumerable<(string title, string url)> appliesTo = null)
        {
            foreach (AnalyzerMetadata analyzer in analyzers)
                WriteAnalyzerMarkdown(analyzer, appliesTo);
        }

        void WriteAnalyzerMarkdown(AnalyzerMetadata analyzer, IEnumerable<(string title, string url)> appliesTo = null)
        {
            WriteAllText(
                $@"..\docs\analyzers\{analyzer.Id}.md",
                MarkdownGenerator.CreateAnalyzerMarkdown(analyzer, metadata.ConfigOptions, appliesTo),
                fileMustExists: false);

            foreach (AnalyzerOptionMetadata option in analyzer.Options
                .Where(f => f.Id is not null))
            {
                WriteAllText(
                    $@"..\docs\analyzers\{option.ParentId}{option.Id}.md",
                    MarkdownGenerator.CreateAnalyzerOptionMarkdown(option),
                    fileMustExists: false);
            }
        }

        void DeleteInvalidAnalyzerMarkdowns()
        {
            AnalyzerMetadata[] allAnalyzers = analyzers
                .Concat(codeAnalysisAnalyzers)
                .Concat(formattingAnalyzers)
                .ToArray();

            IEnumerable<string> allIds = allAnalyzers
                .Concat(allAnalyzers.SelectMany(f => f.OptionAnalyzers))
                .Select(f => f.Id);

            string directoryPath = GetPath(@"..\docs\analyzers");

            foreach (string id in Directory.GetFiles(directoryPath, "*.*", SearchOption.TopDirectoryOnly)
                .Select(f => Path.GetFileNameWithoutExtension(f))
                .Except(allIds))
            {
                if (id == "RCSXXXX")
                    break;

                string filePath = Path.Combine(directoryPath, Path.ChangeExtension(id, ".md"));

                Console.WriteLine($"Delete file '{filePath}'");

                File.Delete(filePath);
            }
        }

        void UpdateChangeLog()
        {
            var issueRegex = new Regex(@"\(\#(?<issue>\d+)\)");
            var analyzerRegex = new Regex(@"(\p{Lu}\p{Ll}+){2,}\ +\((?<id>RCS\d{4}[a-z]?)\)");

            string path = GetPath(@"..\ChangeLog.md");
            string s = File.ReadAllText(path, _utf8NoBom);

            List<AnalyzerMetadata> allAnalyzers = analyzers
                .Concat(formattingAnalyzers)
                .Concat(codeAnalysisAnalyzers)
                .ToList();

            ImmutableDictionary<string, AnalyzerMetadata> dic = allAnalyzers
                .Concat(allAnalyzers.SelectMany(f => f.OptionAnalyzers))
                .Where(f => f.Id is not null)
                .ToImmutableDictionary(f => f.Id, f => f);

            s = issueRegex.Replace(s, "([issue](https://github.com/JosefPihrt/Roslynator/issues/${issue}))");

            s = analyzerRegex.Replace(
                s,
                m =>
                {
                    string id = m.Groups["id"].Value;

                    Debug.Assert(dic.ContainsKey(id), id);

                    AnalyzerMetadata analyzer = dic[id];

                    return $"[{id}](https://github.com/JosefPihrt/Roslynator/blob/main/docs/analyzers/{id}.md) ({analyzer.Title.TrimEnd('.')})";
                });

            File.WriteAllText(path, s, _utf8NoBom);
        }

        void WriteAllText(string relativePath, string content, bool onlyIfChanges = true, bool fileMustExists = true)
        {
            string path = GetPath(relativePath);

            Encoding encoding = (Path.GetExtension(path) == ".md") ? _utf8NoBom : Encoding.UTF8;

            FileHelper.WriteAllText(path, content, encoding, onlyIfChanges, fileMustExists);
        }

        string GetPath(string path)
        {
            return Path.Combine(rootPath, path);
        }
    }
}
