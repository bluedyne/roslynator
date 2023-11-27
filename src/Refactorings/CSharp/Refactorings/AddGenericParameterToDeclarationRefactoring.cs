﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Roslynator.CSharp.CSharpFactory;
using static Roslynator.CSharp.CSharpTypeFactory;

namespace Roslynator.CSharp.Refactorings;

internal static class AddGenericParameterToDeclarationRefactoring
{
    public static void ComputeRefactoring(RefactoringContext context, ClassDeclarationSyntax classDeclaration)
    {
        TypeParameterListSyntax typeParameterList = classDeclaration.TypeParameterList;

        if (typeParameterList is not null)
        {
            if (context.Span.IsEmptyAndContainedInSpan(typeParameterList))
                RegisterRefactoring(context, classDeclaration);
        }
        else
        {
            TextSpan span = context.Span;

            SyntaxToken identifier = classDeclaration.Identifier;

            if (CheckIdentifierAndSpan(identifier, span)
                && span.End <= identifier.GetNextToken().SpanStart)
            {
                RegisterRefactoring(context, classDeclaration);
            }
        }
    }

    public static void ComputeRefactoring(RefactoringContext context, RecordDeclarationSyntax recordDeclaration)
    {
        TypeParameterListSyntax typeParameterList = recordDeclaration.TypeParameterList;

        if (typeParameterList is not null)
        {
            if (context.Span.IsEmptyAndContainedInSpan(typeParameterList))
                RegisterRefactoring(context, recordDeclaration);
        }
        else
        {
            TextSpan span = context.Span;

            SyntaxToken identifier = recordDeclaration.Identifier;

            if (CheckIdentifierAndSpan(identifier, span)
                && span.End <= identifier.GetNextToken().SpanStart)
            {
                RegisterRefactoring(context, recordDeclaration);
            }
        }
    }

    public static void ComputeRefactoring(RefactoringContext context, StructDeclarationSyntax structDeclaration)
    {
        TypeParameterListSyntax typeParameterList = structDeclaration.TypeParameterList;

        if (typeParameterList is not null)
        {
            if (context.Span.IsEmptyAndContainedInSpan(typeParameterList))
                RegisterRefactoring(context, structDeclaration);
        }
        else
        {
            TextSpan span = context.Span;

            SyntaxToken identifier = structDeclaration.Identifier;

            if (!identifier.IsMissing
                && span.Start >= identifier.Span.End
                && CheckIdentifierAndSpan(identifier, span)
                && span.End <= identifier.GetNextToken().SpanStart)
            {
                RegisterRefactoring(context, structDeclaration);
            }
        }
    }

    public static void ComputeRefactoring(RefactoringContext context, InterfaceDeclarationSyntax interfaceDeclaration)
    {
        TypeParameterListSyntax typeParameterList = interfaceDeclaration.TypeParameterList;

        if (typeParameterList is not null)
        {
            if (context.Span.IsEmptyAndContainedInSpan(typeParameterList))
                RegisterRefactoring(context, interfaceDeclaration);
        }
        else
        {
            TextSpan span = context.Span;

            SyntaxToken identifier = interfaceDeclaration.Identifier;

            if (!identifier.IsMissing
                && span.Start >= identifier.Span.End
                && CheckIdentifierAndSpan(identifier, span)
                && span.End <= identifier.GetNextToken().SpanStart)
            {
                RegisterRefactoring(context, interfaceDeclaration);
            }
        }
    }

    public static void ComputeRefactoring(RefactoringContext context, DelegateDeclarationSyntax delegateDeclaration)
    {
        TypeParameterListSyntax typeParameterList = delegateDeclaration.TypeParameterList;

        if (typeParameterList is not null)
        {
            if (context.Span.IsEmptyAndContainedInSpan(typeParameterList))
                RegisterRefactoring(context, delegateDeclaration);
        }
        else
        {
            TextSpan span = context.Span;

            SyntaxToken identifier = delegateDeclaration.Identifier;

            if (!identifier.IsMissing
                && span.Start >= identifier.Span.End
                && CheckIdentifierAndSpan(identifier, span))
            {
                ParameterListSyntax parameterList = delegateDeclaration.ParameterList;

                if (parameterList is not null
                    && span.End <= parameterList.SpanStart)
                {
                    RegisterRefactoring(context, delegateDeclaration);
                }
            }
        }
    }

    public static void ComputeRefactoring(RefactoringContext context, MethodDeclarationSyntax methodDeclaration)
    {
        TypeParameterListSyntax typeParameterList = methodDeclaration.TypeParameterList;

        if (typeParameterList is not null)
        {
            if (context.Span.IsEmptyAndContainedInSpan(typeParameterList))
                RegisterRefactoring(context, methodDeclaration);
        }
        else
        {
            TextSpan span = context.Span;

            SyntaxToken identifier = methodDeclaration.Identifier;

            if (!identifier.IsMissing
                && span.Start >= identifier.Span.End
                && CheckIdentifierAndSpan(identifier, span))
            {
                ParameterListSyntax parameterList = methodDeclaration.ParameterList;

                if (parameterList is not null
                    && span.End <= parameterList.SpanStart
                    && methodDeclaration.BodyOrExpressionBody() is not null)
                {
                    RegisterRefactoring(context, methodDeclaration);
                }
            }
        }
    }

    internal static void ComputeRefactoring(RefactoringContext context, LocalFunctionStatementSyntax localFunctionStatement)
    {
        TypeParameterListSyntax typeParameterList = localFunctionStatement.TypeParameterList;

        if (typeParameterList is not null)
        {
            if (context.Span.IsEmptyAndContainedInSpan(typeParameterList))
                RegisterRefactoring(context, localFunctionStatement);
        }
        else
        {
            TextSpan span = context.Span;

            SyntaxToken identifier = localFunctionStatement.Identifier;

            if (!identifier.IsMissing
                && span.Start >= identifier.Span.End
                && CheckIdentifierAndSpan(identifier, span))
            {
                ParameterListSyntax parameterList = localFunctionStatement.ParameterList;

                if (parameterList is not null
                    && span.End <= parameterList.SpanStart)
                {
                    RegisterRefactoring(context, localFunctionStatement);
                }
            }
        }
    }

    private static bool CheckIdentifierAndSpan(SyntaxToken identifier, TextSpan span)
    {
        if (identifier.IsMissing)
            return false;

        if (span.Start < identifier.Span.End)
            return false;

        foreach (SyntaxTrivia trivia in identifier.TrailingTrivia)
        {
            if (trivia.SpanStart >= span.End)
                break;

            if (!trivia.IsWhitespaceTrivia())
                return false;
        }

        return true;
    }

    private static void RegisterRefactoring(RefactoringContext context, SyntaxNode node)
    {
        context.RegisterRefactoring(
            "Add generic parameter",
            ct => RefactorAsync(context.Document, node, ConstraintKind.None, ct),
            RefactoringDescriptors.AddGenericParameterToDeclaration);

        context.RegisterRefactoring(
            "Add generic parameter with type constraint",
            ct => RefactorAsync(context.Document, node, ConstraintKind.Type, ct),
            RefactoringDescriptors.AddGenericParameterToDeclaration,
            "WithTypeConstraint");
    }

    private static async Task<Document> RefactorAsync(
        Document document,
        SyntaxNode node,
        ConstraintKind constraintKind,
        CancellationToken cancellationToken)
    {
        SemanticModel semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);

        TypeParameterConstraintSyntax constraint = CreateConstraint(constraintKind);

        SyntaxNode newNode = node;

        switch (node.Kind())
        {
            case SyntaxKind.MethodDeclaration:
                {
                    newNode = GetNewNode((MethodDeclarationSyntax)node, constraint, semanticModel, cancellationToken);
                    break;
                }
            case SyntaxKind.ClassDeclaration:
                {
                    newNode = GetNewNode((ClassDeclarationSyntax)node, constraint, semanticModel);
                    break;
                }
            case SyntaxKind.RecordDeclaration:
            case SyntaxKind.RecordStructDeclaration:
                {
                    newNode = GetNewNode((RecordDeclarationSyntax)node, constraint, semanticModel);
                    break;
                }
            case SyntaxKind.StructDeclaration:
                {
                    newNode = GetNewNode((StructDeclarationSyntax)node, constraint, semanticModel);
                    break;
                }
            case SyntaxKind.InterfaceDeclaration:
                {
                    newNode = GetNewNode((InterfaceDeclarationSyntax)node, constraint, semanticModel);
                    break;
                }
            case SyntaxKind.DelegateDeclaration:
                {
                    newNode = GetNewNode((DelegateDeclarationSyntax)node, constraint, semanticModel);
                    break;
                }
            case SyntaxKind.LocalFunctionStatement:
                {
                    newNode = GetNewNode((LocalFunctionStatementSyntax)node, constraint, semanticModel, cancellationToken);
                    break;
                }
        }

        return await document.ReplaceNodeAsync(node, newNode, cancellationToken).ConfigureAwait(false);
    }

    private static MethodDeclarationSyntax GetNewNode(
        MethodDeclarationSyntax methodDeclaration,
        TypeParameterConstraintSyntax constraint,
        SemanticModel semanticModel,
        CancellationToken cancellationToken)
    {
        string name = GetMethodTypeParameterName(semanticModel, methodDeclaration.BodyOrExpressionBody().SpanStart, cancellationToken);

        MethodDeclarationSyntax newNode = methodDeclaration.AddTypeParameterListParameters(TypeParameter(Identifier(name).WithRenameAnnotation()));

        if (constraint is not null)
            newNode = newNode.AddConstraintClauses(TypeParameterConstraintClause(name, constraint));

        return newNode;
    }

    private static ClassDeclarationSyntax GetNewNode(
        ClassDeclarationSyntax classDeclaration,
        TypeParameterConstraintSyntax constraint,
        SemanticModel semanticModel)
    {
        string name = GetTypeParameterName(classDeclaration.OpenBraceToken.SpanStart, semanticModel);

        ClassDeclarationSyntax newNode = classDeclaration.AddTypeParameterListParameters(TypeParameter(Identifier(name).WithRenameAnnotation()));

        if (constraint is not null)
            newNode = newNode.AddConstraintClauses(TypeParameterConstraintClause(name, constraint));

        return newNode;
    }

    private static RecordDeclarationSyntax GetNewNode(
        RecordDeclarationSyntax recordDeclaration,
        TypeParameterConstraintSyntax constraint,
        SemanticModel semanticModel)
    {
        int position = (recordDeclaration.OpenBraceToken != default)
            ? recordDeclaration.OpenBraceToken.SpanStart
            : recordDeclaration.SemicolonToken.SpanStart;

        string name = GetTypeParameterName(position, semanticModel);

        RecordDeclarationSyntax newNode = recordDeclaration.AddTypeParameterListParameters(TypeParameter(Identifier(name).WithRenameAnnotation()));

        if (constraint is not null)
            newNode = newNode.AddConstraintClauses(TypeParameterConstraintClause(name, constraint));

        return newNode;
    }

    private static StructDeclarationSyntax GetNewNode(
        StructDeclarationSyntax structDeclaration,
        TypeParameterConstraintSyntax typeParameterConstraint,
        SemanticModel semanticModel)
    {
        string name = GetTypeParameterName(structDeclaration.OpenBraceToken.SpanStart, semanticModel);

        StructDeclarationSyntax newNode = structDeclaration.AddTypeParameterListParameters(TypeParameter(Identifier(name).WithRenameAnnotation()));

        if (typeParameterConstraint is not null)
            newNode = newNode.AddConstraintClauses(TypeParameterConstraintClause(name, typeParameterConstraint));

        return newNode;
    }

    private static InterfaceDeclarationSyntax GetNewNode(
        InterfaceDeclarationSyntax interfaceDeclaration,
        TypeParameterConstraintSyntax constraint,
        SemanticModel semanticModel)
    {
        string name = GetTypeParameterName(interfaceDeclaration.OpenBraceToken.SpanStart, semanticModel);

        InterfaceDeclarationSyntax newNode = interfaceDeclaration.AddTypeParameterListParameters(TypeParameter(Identifier(name).WithRenameAnnotation()));

        if (constraint is not null)
            newNode = newNode.AddConstraintClauses(TypeParameterConstraintClause(name, constraint));

        return newNode;
    }

    private static DelegateDeclarationSyntax GetNewNode(
        DelegateDeclarationSyntax delegateDeclaration,
        TypeParameterConstraintSyntax constraint,
        SemanticModel semanticModel)
    {
        TypeParameterListSyntax typeParameterList = delegateDeclaration.TypeParameterList;

        int position = (typeParameterList is not null)
            ? typeParameterList.SpanStart
            : delegateDeclaration.Identifier.SpanStart;

        string name = GetTypeParameterName(position, semanticModel);

        DelegateDeclarationSyntax newNode = delegateDeclaration.AddTypeParameterListParameters(TypeParameter(Identifier(name).WithRenameAnnotation()));

        if (constraint is not null)
            newNode = newNode.AddConstraintClauses(TypeParameterConstraintClause(name, constraint));

        return newNode;
    }

    private static LocalFunctionStatementSyntax GetNewNode(
        LocalFunctionStatementSyntax localFunctionStatement,
        TypeParameterConstraintSyntax constraint,
        SemanticModel semanticModel,
        CancellationToken cancellationToken)
    {
        string name = GetMethodTypeParameterName(semanticModel, localFunctionStatement.BodyOrExpressionBody().SpanStart, cancellationToken);

        LocalFunctionStatementSyntax newNode = localFunctionStatement.AddTypeParameterListParameters(TypeParameter(Identifier(name).WithRenameAnnotation()));

        if (constraint is not null)
            newNode = newNode.AddConstraintClauses(TypeParameterConstraintClause(name, constraint));

        return newNode;
    }

    private static string GetTypeParameterName(int position, SemanticModel semanticModel)
    {
        return NameGenerator.Default.EnsureUniqueName(
            DefaultNames.TypeParameter,
            semanticModel.LookupSymbols(position));
    }

    private static string GetMethodTypeParameterName(SemanticModel semanticModel, int position, CancellationToken cancellationToken)
    {
        return NameGenerator.Default.EnsureUniqueLocalName(
            DefaultNames.TypeParameter,
            semanticModel,
            position,
            cancellationToken: cancellationToken);
    }

    private static TypeParameterConstraintSyntax CreateConstraint(ConstraintKind constraintKind)
    {
        switch (constraintKind)
        {
            case ConstraintKind.Type:
                return TypeConstraint(ObjectType());
            case ConstraintKind.Class:
                return ClassConstraint();
            case ConstraintKind.Struct:
                return StructConstraint();
            case ConstraintKind.Constructor:
                return ConstructorConstraint();
            default:
                return null;
        }
    }

    private enum ConstraintKind
    {
        None,
        Type,
        Class,
        Struct,
        Constructor,
    }
}
