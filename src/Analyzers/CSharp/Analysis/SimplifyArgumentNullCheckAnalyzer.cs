﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp.Syntax;

namespace Roslynator.CSharp.Analysis;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class SimplifyArgumentNullCheckAnalyzer : BaseDiagnosticAnalyzer
{
    private static ImmutableArray<DiagnosticDescriptor> _supportedDiagnostics;

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
    {
        get
        {
            if (_supportedDiagnostics.IsDefault)
                Immutable.InterlockedInitialize(ref _supportedDiagnostics, DiagnosticRules.SimplifyArgumentNullCheck);

            return _supportedDiagnostics;
        }
    }

    public override void Initialize(AnalysisContext context)
    {
        base.Initialize(context);

        context.RegisterSyntaxNodeAction(f => AnalyzeIfStatement(f), SyntaxKind.IfStatement);
    }

    private static void AnalyzeIfStatement(SyntaxNodeAnalysisContext context)
    {
        var ifStatement = (IfStatementSyntax)context.Node;

        if (!ifStatement.IsSimpleIf())
            return;

        NullCheckExpressionInfo nullCheck = SyntaxInfo.NullCheckExpressionInfo(
            ifStatement.Condition,
            NullCheckStyles.EqualsToNull | NullCheckStyles.IsNull);

        if (!nullCheck.Success)
            return;

        if (nullCheck.Expression is not IdentifierNameSyntax identifierName)
            return;

        if (ifStatement.SingleNonBlockStatementOrDefault() is not ThrowStatementSyntax throwStatement)
            return;

        if (throwStatement.Expression is not ObjectCreationExpressionSyntax objectCreation)
            return;

        ExpressionSyntax expression = objectCreation.ArgumentList?.Arguments.SingleOrDefault(shouldThrow: false)?.Expression;

        if (expression is null)
            return;

        if (ifStatement.ContainsUnbalancedIfElseDirectives())
            return;

        INamedTypeSymbol containingTypeSymbol = context.SemanticModel
            .GetSymbol(objectCreation, context.CancellationToken)?
            .ContainingType;

        if (containingTypeSymbol?.HasMetadataName(MetadataNames.System_ArgumentNullException) != true)
            return;

        if (!containingTypeSymbol.GetMembers("ThrowIfNull").Any(f => f.IsKind(SymbolKind.Method)))
            return;

        if (expression.IsKind(SyntaxKind.StringLiteralExpression))
        {
            var literal = (LiteralExpressionSyntax)expression;

            if (string.Equals(identifierName.Identifier.ValueText, literal.Token.ValueText))
                ReportDiagnostic(context, ifStatement);
        }
        else if (expression is InvocationExpressionSyntax invocationExpression)
        {
            if (CSharpUtility.IsNameOfExpression(invocationExpression, context.SemanticModel, context.CancellationToken)
                && invocationExpression.ArgumentList.Arguments.FirstOrDefault().Expression is IdentifierNameSyntax identifierName2
                && string.Equals(identifierName.Identifier.ValueText, identifierName2.Identifier.ValueText))
            {
                ReportDiagnostic(context, ifStatement);
            }
        }
    }

    private static void ReportDiagnostic(SyntaxNodeAnalysisContext context, IfStatementSyntax ifStatement)
    {
        context.ReportDiagnostic(DiagnosticRules.SimplifyArgumentNullCheck, ifStatement.IfKeyword);
    }
}
