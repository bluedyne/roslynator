﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp;

namespace Roslynator.Formatting.CSharp;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class PutTypeParameterConstraintOnItsOwnLineAnalyzer : BaseDiagnosticAnalyzer
{
    private static ImmutableArray<DiagnosticDescriptor> _supportedDiagnostics;

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
    {
        get
        {
            if (_supportedDiagnostics.IsDefault)
                Immutable.InterlockedInitialize(ref _supportedDiagnostics, DiagnosticRules.PutTypeParameterConstraintOnItsOwnLine);

            return _supportedDiagnostics;
        }
    }

    public override void Initialize(AnalysisContext context)
    {
        base.Initialize(context);

        context.RegisterSyntaxNodeAction(
            f => AnalyzeTypeDeclaration(f),
            SyntaxKind.ClassDeclaration,
            SyntaxKind.StructDeclaration,
            SyntaxKind.RecordStructDeclaration,
            SyntaxKind.InterfaceDeclaration);

        context.RegisterSyntaxNodeAction(f => AnalyzeDelegateDeclaration(f), SyntaxKind.DelegateDeclaration);
        context.RegisterSyntaxNodeAction(f => AnalyzeMethodDeclaration(f), SyntaxKind.MethodDeclaration);
        context.RegisterSyntaxNodeAction(f => AnalyzeLocalFunctionStatement(f), SyntaxKind.LocalFunctionStatement);
    }

    private static void AnalyzeTypeDeclaration(SyntaxNodeAnalysisContext context)
    {
        var typeDeclaration = (TypeDeclarationSyntax)context.Node;

        SyntaxList<TypeParameterConstraintClauseSyntax> constraintClauses = typeDeclaration.ConstraintClauses;

        TypeParameterListSyntax typeParameterList = typeDeclaration.TypeParameterList;

        if (typeParameterList is null)
            return;

        Analyze(context, typeParameterList.GreaterThanToken, constraintClauses);
    }

    private static void AnalyzeDelegateDeclaration(SyntaxNodeAnalysisContext context)
    {
        var delegateDeclaration = (DelegateDeclarationSyntax)context.Node;

        SyntaxList<TypeParameterConstraintClauseSyntax> constraintClauses = delegateDeclaration.ConstraintClauses;

        Analyze(context, delegateDeclaration.ParameterList.CloseParenToken, constraintClauses);
    }

    private static void AnalyzeMethodDeclaration(SyntaxNodeAnalysisContext context)
    {
        var methodDeclaration = (MethodDeclarationSyntax)context.Node;

        SyntaxList<TypeParameterConstraintClauseSyntax> constraintClauses = methodDeclaration.ConstraintClauses;

        Analyze(context, methodDeclaration.ParameterList.CloseParenToken, constraintClauses);
    }

    private static void AnalyzeLocalFunctionStatement(SyntaxNodeAnalysisContext context)
    {
        var localFunction = (LocalFunctionStatementSyntax)context.Node;

        SyntaxList<TypeParameterConstraintClauseSyntax> constraintClauses = localFunction.ConstraintClauses;

        Analyze(context, localFunction.ParameterList.CloseParenToken, constraintClauses);
    }

    private static void Analyze(
        SyntaxNodeAnalysisContext context,
        SyntaxNodeOrToken previous,
        SyntaxList<TypeParameterConstraintClauseSyntax> constraintClauses)
    {
        if (constraintClauses.Count <= 1)
            return;

        foreach (TypeParameterConstraintClauseSyntax constraintClause in constraintClauses)
        {
            TriviaBlock block = TriviaBlock.FromBetween(previous, constraintClause);

            if (block.Kind == TriviaBlockKind.NoNewLine)
            {
                DiagnosticHelpers.ReportDiagnostic(
                    context,
                    DiagnosticRules.PutTypeParameterConstraintOnItsOwnLine,
                    block.GetLocation());
            }

            previous = constraintClause;
        }
    }
}
