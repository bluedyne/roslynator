﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp;

namespace Roslynator.Formatting.CSharp;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class PutStatementOnItsOwnLineAnalyzer : BaseDiagnosticAnalyzer
{
    private static ImmutableArray<DiagnosticDescriptor> _supportedDiagnostics;

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
    {
        get
        {
            if (_supportedDiagnostics.IsDefault)
                Immutable.InterlockedInitialize(ref _supportedDiagnostics, DiagnosticRules.PutStatementOnItsOwnLine);

            return _supportedDiagnostics;
        }
    }

    public override void Initialize(AnalysisContext context)
    {
        base.Initialize(context);

        context.RegisterSyntaxNodeAction(f => AnalyzeBlock(f), SyntaxKind.Block);
        context.RegisterSyntaxNodeAction(f => AnalyzeSwitchSection(f), SyntaxKind.SwitchSection);
    }

    private static void AnalyzeBlock(SyntaxNodeAnalysisContext context)
    {
        var block = (BlockSyntax)context.Node;

        Analyze(context, block.Statements);
    }

    private static void AnalyzeSwitchSection(SyntaxNodeAnalysisContext context)
    {
        var switchSection = (SwitchSectionSyntax)context.Node;

        Analyze(context, switchSection.Statements);
    }

    private static void Analyze(SyntaxNodeAnalysisContext context, SyntaxList<StatementSyntax> statements)
    {
        if (statements.Count <= 1)
            return;

        StatementSyntax first = statements.FirstOrDefault();

        if (first is null)
            return;

        SyntaxNodeOrToken previous = first.GetFirstToken().GetPreviousToken();

        for (int i = 0; i < statements.Count; i++)
        {
            StatementSyntax statement = statements[i];
            if (!statement.IsKind(SyntaxKind.Block, SyntaxKind.EmptyStatement))
            {
                TriviaBlock block = TriviaBlock.FromBetween(previous, statement);

                if (block.Kind == TriviaBlockKind.NoNewLine)
                {
                    DiagnosticHelpers.ReportDiagnostic(
                        context,
                        DiagnosticRules.PutStatementOnItsOwnLine,
                        block.GetLocation());
                }
            }

            previous = statement;
        }
    }
}
