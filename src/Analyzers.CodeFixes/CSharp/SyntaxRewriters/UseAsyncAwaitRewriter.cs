﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Roslynator;
using Roslynator.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Roslynator.CSharp.CSharpFactory;

namespace Roslynator.CSharp.SyntaxRewriters;

internal class UseAsyncAwaitRewriter : SkipFunctionRewriter
{
    private static readonly SyntaxAnnotation[] _asyncAwaitAnnotation = new[] { new SyntaxAnnotation() };

    private static readonly SyntaxAnnotation[] _asyncAwaitAnnotationAndFormatterAnnotation = new SyntaxAnnotation[] { _asyncAwaitAnnotation[0], Formatter.Annotation };

    public UseAsyncAwaitRewriter(bool keepReturnStatement)
    {
        KeepReturnStatement = keepReturnStatement;
    }

    public bool KeepReturnStatement { get; }

    public static UseAsyncAwaitRewriter Create(IMethodSymbol methodSymbol)
    {
        ITypeSymbol returnType = methodSymbol.ReturnType.OriginalDefinition;

        var keepReturnStatement = false;

        if (returnType.EqualsOrInheritsFrom(MetadataNames.System_Threading_Tasks_ValueTask_T)
            || returnType.EqualsOrInheritsFrom(MetadataNames.System_Threading_Tasks_Task_T))
        {
            keepReturnStatement = true;
        }

        return new UseAsyncAwaitRewriter(keepReturnStatement: keepReturnStatement);
    }

    public override SyntaxNode VisitReturnStatement(ReturnStatementSyntax node)
    {
        ExpressionSyntax expression = node.Expression;

        if (expression?.IsKind(SyntaxKind.AwaitExpression) == false)
        {
            if (KeepReturnStatement)
            {
                return node.WithExpression(AwaitExpression(expression.WithoutTrivia().Parenthesize()).WithTriviaFrom(expression));
            }
            else
            {
                return ExpressionStatement(AwaitExpression(expression.WithoutTrivia().Parenthesize()).WithTriviaFrom(expression))
                    .WithLeadingTrivia(node.GetLeadingTrivia())
                    .WithAdditionalAnnotations(_asyncAwaitAnnotationAndFormatterAnnotation);
            }
        }

        return base.VisitReturnStatement(node);
    }

    public override SyntaxNode VisitBlock(BlockSyntax node)
    {
        var newNode = (BlockSyntax)base.VisitBlock(node);

        SyntaxList<StatementSyntax> statements = newNode.Statements;

        statements = RewriteStatements(node, statements);

        return newNode.WithStatements(statements);
    }

    public override SyntaxNode VisitSwitchSection(SwitchSectionSyntax node)
    {
        var newNode = (SwitchSectionSyntax)base.VisitSwitchSection(node);

        SyntaxList<StatementSyntax> statements = newNode.Statements;

        statements = RewriteStatements(node, statements);

        return newNode.WithStatements(statements);
    }

    private static SyntaxList<StatementSyntax> RewriteStatements(SyntaxNode parent, SyntaxList<StatementSyntax> statements)
    {
        if (!statements.Any())
            return statements;

        int startIndex = statements.Count - 1;

        if (parent.IsKind(SyntaxKind.Block)
            && (parent.IsParentKind(
                SyntaxKind.MethodDeclaration,
                SyntaxKind.LocalFunctionStatement,
                SyntaxKind.SimpleLambdaExpression,
                SyntaxKind.ParenthesizedLambdaExpression,
                SyntaxKind.AnonymousMethodExpression)
                || (parent.IsParentKind(SyntaxKind.UsingStatement)
                    && parent.Parent.IsParentKind(SyntaxKind.Block)
                    && parent.Parent.Parent.IsParentKind(
                        SyntaxKind.MethodDeclaration,
                        SyntaxKind.LocalFunctionStatement,
                        SyntaxKind.SimpleLambdaExpression,
                        SyntaxKind.ParenthesizedLambdaExpression,
                        SyntaxKind.AnonymousMethodExpression))))
        {
            if (startIndex == 0)
                return statements;

            startIndex--;
        }

        for (int i = startIndex; i >= 0; i--)
        {
            StatementSyntax statement = statements[i];

            if (statement.HasAnnotation(_asyncAwaitAnnotation[0]))
            {
                statements = statements.Replace(
                    statement,
                    statement.WithoutAnnotations(_asyncAwaitAnnotation).WithTrailingTrivia(NewLine()));

                statements = statements.Insert(
                    i + 1,
                    ReturnStatement().WithTrailingTrivia(statement.GetTrailingTrivia()).WithFormatterAnnotation());
            }
        }

        return statements;
    }
}
