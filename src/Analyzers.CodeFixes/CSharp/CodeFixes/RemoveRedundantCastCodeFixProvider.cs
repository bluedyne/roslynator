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

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(RemoveRedundantCastCodeFixProvider))]
[Shared]
public sealed class RemoveRedundantCastCodeFixProvider : BaseCodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds
    {
        get { return ImmutableArray.Create(DiagnosticIdentifiers.RemoveRedundantCast); }
    }

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        SyntaxNode root = await context.GetSyntaxRootAsync().ConfigureAwait(false);

        if (!TryFindFirstAncestorOrSelf(root, context.Span, out SyntaxNode node, predicate: f => f.IsKind(SyntaxKind.CastExpression, SyntaxKind.InvocationExpression)))
            return;

        switch (node.Kind())
        {
            case SyntaxKind.CastExpression:
                {
                    CodeAction codeAction = CodeAction.Create(
                        "Remove redundant cast",
                        ct => RemoveRedundantCastRefactoring.RefactorAsync(context.Document, (CastExpressionSyntax)node, ct),
                        GetEquivalenceKey(DiagnosticIdentifiers.RemoveRedundantCast));

                    context.RegisterCodeFix(codeAction, context.Diagnostics);
                    break;
                }
            case SyntaxKind.InvocationExpression:
                {
                    CodeAction codeAction = CodeAction.Create(
                        "Remove redundant cast",
                        ct => RemoveRedundantCastRefactoring.RefactorAsync(context.Document, (InvocationExpressionSyntax)node, ct),
                        GetEquivalenceKey(DiagnosticIdentifiers.RemoveRedundantCast));

                    context.RegisterCodeFix(codeAction, context.Diagnostics);
                    break;
                }
        }
    }
}
