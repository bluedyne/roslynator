﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Composition;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslynator.CodeFixes;

namespace Roslynator.CSharp.CodeFixes;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(DeclareAsNullableCodeFixProvider))]
[Shared]
public sealed class DeclareAsNullableCodeFixProvider : CompilerDiagnosticCodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds
    {
        get { return ImmutableArray.Create(CompilerDiagnosticIdentifiers.CS8600_ConvertingNullLiteralOrPossibleNullValueToNonNullableType); }
    }

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        Diagnostic diagnostic = context.Diagnostics[0];

        SyntaxNode root = await context.GetSyntaxRootAsync().ConfigureAwait(false);

        if (!IsEnabled(diagnostic.Id, CodeFixIdentifiers.AddNullableAnnotation, context.Document, root.SyntaxTree))
            return;

        if (!TryFindFirstAncestorOrSelf(root, context.Span, out SyntaxNode node, predicate: f => f.IsKind(SyntaxKind.EqualsValueClause, SyntaxKind.DeclarationExpression)))
            return;

        if (node is EqualsValueClauseSyntax equalsValueClause)
        {
            ExpressionSyntax expression = equalsValueClause.Value;

            if (expression.Span == context.Span
                && equalsValueClause.IsParentKind(SyntaxKind.VariableDeclarator)
                && equalsValueClause.Parent.Parent is VariableDeclarationSyntax variableDeclaration
                && variableDeclaration.Variables.Count == 1)
            {
                TypeSyntax type = variableDeclaration.Type;

                if (!type.IsKind(SyntaxKind.NullableType))
                    TryRegisterCodeFix(context, diagnostic, type);
            }
        }
        else if (node is DeclarationExpressionSyntax declarationExpression)
        {
            TryRegisterCodeFix(context, diagnostic, declarationExpression.Type);
        }
    }

    private static void TryRegisterCodeFix(CodeFixContext context, Diagnostic diagnostic, TypeSyntax type)
    {
        if (!type.IsKind(SyntaxKind.NullableType))
        {
            CodeAction codeAction = CodeAction.Create(
                "Declare as nullable",
                ct =>
                {
                    NullableTypeSyntax newType = SyntaxFactory.NullableType(type.WithoutTrivia()).WithTriviaFrom(type);
                    return context.Document.ReplaceNodeAsync(type, newType, ct);
                },
                GetEquivalenceKey(diagnostic));

            context.RegisterCodeFix(codeAction, diagnostic);
        }
    }
}
