﻿// Copyright (c) Josef Pihrt and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Roslynator.CSharp.CSharpFactory;

namespace Roslynator.CSharp.Refactorings;

internal static class PromoteLocalToParameterRefactoring
{
    public static void ComputeRefactoring(
        RefactoringContext context,
        LocalDeclarationStatementSyntax localDeclaration,
        SemanticModel semanticModel)
    {
        if (semanticModel.GetEnclosingSymbol(localDeclaration.SpanStart, context.CancellationToken) is not IMethodSymbol methodSymbol)
            return;

        if (methodSymbol.IsImplicitlyDeclared)
            return;

        if (!methodSymbol.MethodKind.Is(MethodKind.Ordinary, MethodKind.LocalFunction))
            return;

        if (methodSymbol.PartialImplementationPart is not null)
            methodSymbol = methodSymbol.PartialImplementationPart;

        SyntaxNode methodOrLocalFunction = methodSymbol.GetSyntax(context.CancellationToken);

        if (!methodOrLocalFunction.IsKind(SyntaxKind.MethodDeclaration, SyntaxKind.LocalFunctionStatement))
            return;

        VariableDeclarationSyntax declaration = localDeclaration.Declaration;

        if (declaration is null)
            return;

        VariableDeclaratorSyntax variable = declaration
            .Variables
            .FirstOrDefault(f => !f.IsMissing && f.Identifier.Span.Contains(context.Span));

        if (variable is null)
            return;

        TypeSyntax type = declaration.Type;

        if (type is null)
            return;

        if (type.IsVar)
        {
            ITypeSymbol typeSymbol = semanticModel.GetTypeSymbol(type, context.CancellationToken);

            if (typeSymbol?.SupportsExplicitDeclaration() == true)
            {
                type = typeSymbol.ToTypeSyntax();
            }
            else
            {
                return;
            }
        }

        context.RegisterRefactoring(
            $"Promote '{variable.Identifier.ValueText}' to parameter",
            ct =>
            {
                return RefactorAsync(
                    context.Document,
                    methodOrLocalFunction,
                    localDeclaration,
                    type.WithoutTrivia().WithSimplifierAnnotation(),
                    variable,
                    ct);
            },
            RefactoringDescriptors.PromoteLocalVariableToParameter);
    }

    public static Task<Document> RefactorAsync(
        Document document,
        SyntaxNode methodOrLocalFunction,
        LocalDeclarationStatementSyntax localDeclaration,
        TypeSyntax type,
        VariableDeclaratorSyntax variable,
        CancellationToken cancellationToken = default)
    {
        int variableCount = localDeclaration.Declaration.Variables.Count;
        ExpressionSyntax initializerValue = variable.Initializer?.Value;
        SyntaxToken identifier = variable.Identifier.WithoutTrivia();

        SyntaxNode newNode = methodOrLocalFunction;

        if (initializerValue is not null)
        {
            ExpressionStatementSyntax expressionStatement = SimpleAssignmentStatement(
                IdentifierName(identifier),
                initializerValue);

            expressionStatement = expressionStatement.WithFormatterAnnotation();

            if (variableCount > 1)
            {
                LocalDeclarationStatementSyntax newLocalDeclaration = localDeclaration.RemoveNode(
                    variable,
                    SyntaxRemoveOptions.KeepUnbalancedDirectives);

                newNode = newNode.ReplaceNode(
                    localDeclaration,
                    new SyntaxNode[] { newLocalDeclaration, expressionStatement });
            }
            else
            {
                newNode = newNode.ReplaceNode(
                    localDeclaration,
                    expressionStatement.WithTriviaFrom(localDeclaration));
            }
        }
        else if (variableCount > 1)
        {
            newNode = newNode.RemoveNode(variable, SyntaxRemoveOptions.KeepUnbalancedDirectives);
        }
        else
        {
            newNode = newNode.RemoveNode(localDeclaration, SyntaxRemoveOptions.KeepUnbalancedDirectives);
        }

        ParameterSyntax newParameter = Parameter(type, identifier).WithFormatterAnnotation();

        if (newNode is MethodDeclarationSyntax methodDeclaration)
        {
            newNode = methodDeclaration.AddParameterListParameters(newParameter);
        }
        else
        {
            var localFunction = (LocalFunctionStatementSyntax)newNode;
            newNode = localFunction.AddParameterListParameters(newParameter);
        }

        return document.ReplaceNodeAsync(methodOrLocalFunction, newNode, cancellationToken);
    }
}
