﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslynator.CSharp.Analysis;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class MergeElseWithNestedIfAnalyzer : BaseDiagnosticAnalyzer
{
    private static ImmutableArray<DiagnosticDescriptor> _supportedDiagnostics;

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
    {
        get
        {
            if (_supportedDiagnostics.IsDefault)
            {
                Immutable.InterlockedInitialize(
                    ref _supportedDiagnostics,
                    DiagnosticRules.MergeElseWithNestedIf,
                    DiagnosticRules.MergeElseWithNestedIfFadeOut);
            }

            return _supportedDiagnostics;
        }
    }

    public override void Initialize(AnalysisContext context)
    {
        base.Initialize(context);

        context.RegisterSyntaxNodeAction(
            c =>
            {
                if (DiagnosticRules.MergeElseWithNestedIf.IsEffective(c))
                    AnalyzeElseClause(c);
            },
            SyntaxKind.ElseClause);
    }

    private static void AnalyzeElseClause(SyntaxNodeAnalysisContext context)
    {
        var elseClause = (ElseClauseSyntax)context.Node;

        if (elseClause.Statement is not BlockSyntax block)
            return;

        if (block.Statements.SingleOrDefault(shouldThrow: false) is not IfStatementSyntax ifStatement)
            return;

        if (!elseClause.ElseKeyword.TrailingTrivia.IsEmptyOrWhitespace()
            || !block.OpenBraceToken.LeadingTrivia.IsEmptyOrWhitespace()
            || !block.OpenBraceToken.TrailingTrivia.IsEmptyOrWhitespace()
            || !ifStatement.IfKeyword.LeadingTrivia.IsEmptyOrWhitespace()
            || !ifStatement.GetTrailingTrivia().IsEmptyOrWhitespace()
            || !block.CloseBraceToken.LeadingTrivia.IsEmptyOrWhitespace())
        {
            return;
        }

        DiagnosticHelpers.ReportDiagnostic(context, DiagnosticRules.MergeElseWithNestedIf, block);
        CSharpDiagnosticHelpers.ReportBraces(context, DiagnosticRules.MergeElseWithNestedIfFadeOut, block);
    }
}
