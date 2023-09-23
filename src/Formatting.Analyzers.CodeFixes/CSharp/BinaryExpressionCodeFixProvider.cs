﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Composition;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslynator.Formatting.CSharp;

namespace Roslynator.Formatting.CodeFixes.CSharp;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(BinaryExpressionCodeFixProvider))]
[Shared]
public sealed class BinaryExpressionCodeFixProvider : BaseCodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds
    {
        get { return ImmutableArray.Create(DiagnosticIdentifiers.PlaceNewLineAfterOrBeforeBinaryOperator); }
    }

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        SyntaxNode root = await context.GetSyntaxRootAsync().ConfigureAwait(false);

        if (!TryFindFirstAncestorOrSelf(root, context.Span, out BinaryExpressionSyntax binaryExpression))
            return;

        Document document = context.Document;
        Diagnostic diagnostic = context.Diagnostics[0];

        if (DiagnosticProperties.ContainsInvert(diagnostic.Properties))
        {
            CodeAction codeAction = CodeAction.Create(
                $"Place new line after '{binaryExpression.OperatorToken.ToString()}'",
                ct => CodeFixHelpers.AddNewLineAfterInsteadOfBeforeAsync(document, binaryExpression.Left, binaryExpression.OperatorToken, binaryExpression.Right, ct),
                GetEquivalenceKey(diagnostic));

            context.RegisterCodeFix(codeAction, diagnostic);
        }
        else
        {
            CodeAction codeAction = CodeAction.Create(
                $"Place new line before '{binaryExpression.OperatorToken.ToString()}'",
                ct => CodeFixHelpers.AddNewLineBeforeInsteadOfAfterAsync(document, binaryExpression.Left, binaryExpression.OperatorToken, binaryExpression.Right, ct),
                GetEquivalenceKey(diagnostic));

            context.RegisterCodeFix(codeAction, diagnostic);
        }
    }
}
