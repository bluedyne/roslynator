﻿// Copyright (c) Josef Pihrt and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Composition;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Roslynator.CodeFixes;

namespace Roslynator.CSharp.CodeFixes;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(AddArgumentListToObjectCreationCodeFixProvider))]
[Shared]
public sealed class AddArgumentListToObjectCreationCodeFixProvider : CompilerDiagnosticCodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds
    {
        get { return ImmutableArray.Create(CompilerDiagnosticIdentifiers.CS1526_NewExpressionRequiresParenthesesOrBracketsOrBracesAfterType); }
    }

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        Diagnostic diagnostic = context.Diagnostics[0];

        SyntaxNode root = await context.GetSyntaxRootAsync().ConfigureAwait(false);

        if (!IsEnabled(diagnostic.Id, CodeFixIdentifiers.AddArgumentList, context.Document, root.SyntaxTree))
            return;

        if (!TryFindFirstAncestorOrSelf(root, new TextSpan(context.Span.Start - 1, 0), out ObjectCreationExpressionSyntax objectCreation))
            return;

        SemanticModel semanticModel = await context.GetSemanticModelAsync().ConfigureAwait(false);

        ISymbol symbol = semanticModel.GetSymbol(objectCreation.Type, context.CancellationToken);

        if (symbol?.IsErrorType() != false)
            return;

        CodeAction codeAction = CodeAction.Create(
            "Add argument list",
            ct =>
            {
                ObjectCreationExpressionSyntax newNode = objectCreation.Update(
                    objectCreation.NewKeyword,
                    objectCreation.Type.WithoutTrailingTrivia(),
                    SyntaxFactory.ArgumentList().WithTrailingTrivia(objectCreation.Type.GetTrailingTrivia()),
                    objectCreation.Initializer);

                return context.Document.ReplaceNodeAsync(objectCreation, newNode, ct);
            },
            GetEquivalenceKey(diagnostic));

        context.RegisterCodeFix(codeAction, diagnostic);
    }
}
