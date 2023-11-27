﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Roslynator.CSharp.CSharpFactory;

namespace Roslynator.CSharp.Refactorings;

internal static class CallIndexOfInsteadOfContainsRefactoring
{
    public static async Task ComputeRefactoringAsync(RefactoringContext context, InvocationExpressionSyntax invocation)
    {
        SemanticModel semanticModel = await context.GetSemanticModelAsync().ConfigureAwait(false);

        IMethodSymbol methodSymbol = semanticModel.GetMethodSymbol(invocation, context.CancellationToken);

        if (SymbolUtility.IsPublicInstanceNonGeneric(methodSymbol, "Contains")
            && methodSymbol.IsContainingType(SpecialType.System_String)
            && methodSymbol.HasSingleParameter(SpecialType.System_String))
        {
            context.RegisterRefactoring(
                "Call 'IndexOf' instead of 'Contains'",
                ct => RefactorAsync(context.Document, invocation, ct),
                RefactoringDescriptors.CallIndexOfInsteadOfContains);
        }
    }

    private static Task<Document> RefactorAsync(
        Document document,
        InvocationExpressionSyntax invocation,
        CancellationToken cancellationToken)
    {
        var memberAccess = (MemberAccessExpressionSyntax)invocation.Expression;

        InvocationExpressionSyntax newInvocationExpression = invocation
            .WithExpression(memberAccess.WithName(IdentifierName("IndexOf")))
            .AddArgumentListArguments(
                Argument(
                    ParseName("System.StringComparison.OrdinalIgnoreCase").WithSimplifierAnnotation()));

        SyntaxNode parent = invocation.Parent;

        if (parent.IsKind(SyntaxKind.LogicalNotExpression))
        {
            BinaryExpressionSyntax equalsExpression = EqualsExpression(newInvocationExpression, NumericLiteralExpression(-1))
                .WithTriviaFrom(parent)
                .WithFormatterAnnotation();

            return document.ReplaceNodeAsync(parent, equalsExpression, cancellationToken);
        }
        else
        {
            BinaryExpressionSyntax notEqualsExpression = NotEqualsExpression(newInvocationExpression, NumericLiteralExpression(-1))
                .WithTriviaFrom(invocation)
                .WithFormatterAnnotation();

            return document.ReplaceNodeAsync(invocation, notEqualsExpression, cancellationToken);
        }
    }
}
