﻿// Copyright (c) Josef Pihrt and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Composition;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslynator.CodeFixes;
using Roslynator.CSharp.Refactorings;

namespace Roslynator.CSharp.CodeFixes;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(EndRegionDirectiveTriviaCodeFixProvider))]
[Shared]
public sealed class EndRegionDirectiveTriviaCodeFixProvider : BaseCodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds
    {
        get { return ImmutableArray.Create(DiagnosticIdentifiers.AddOrRemoveRegionName); }
    }

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        SyntaxNode root = await context.GetSyntaxRootAsync().ConfigureAwait(false);

        if (!TryFindFirstAncestorOrSelf(root, context.Span, out EndRegionDirectiveTriviaSyntax endRegionDirective, findInsideTrivia: true))
            return;

        RegionDirectiveTriviaSyntax regionDirective = endRegionDirective.GetRegionDirective();

        SyntaxTrivia trivia = regionDirective.GetPreprocessingMessageTrivia();

        CodeAction codeAction = CodeAction.Create(
            (trivia.IsKind(SyntaxKind.PreprocessingMessageTrivia))
                ? "Add region name to #endregion"
                : "Remove region name from #endregion",
            ct => AddOrRemoveRegionNameRefactoring.RefactorAsync(context.Document, endRegionDirective, trivia, ct),
            GetEquivalenceKey(DiagnosticIdentifiers.AddOrRemoveRegionName));

        context.RegisterCodeFix(codeAction, context.Diagnostics);
    }
}
