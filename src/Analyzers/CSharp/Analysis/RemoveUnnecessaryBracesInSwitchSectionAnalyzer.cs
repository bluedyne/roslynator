﻿// Copyright (c) Josef Pihrt and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslynator.CSharp.Analysis;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class RemoveUnnecessaryBracesInSwitchSectionAnalyzer : BaseDiagnosticAnalyzer
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
                    DiagnosticRules.RemoveUnnecessaryBracesInSwitchSection,
                    DiagnosticRules.RemoveUnnecessaryBracesInSwitchSectionFadeOut);
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
                if (DiagnosticRules.RemoveUnnecessaryBracesInSwitchSection.IsEffective(c))
                    AnalyzerSwitchSection(c);
            },
            SyntaxKind.SwitchSection);
    }

    public static void AnalyzerSwitchSection(SyntaxNodeAnalysisContext context)
    {
        var switchSection = (SwitchSectionSyntax)context.Node;

        if (switchSection.Statements.SingleOrDefault(shouldThrow: false) is not BlockSyntax block)
            return;

        SyntaxList<StatementSyntax> statements = block.Statements;

        SyntaxList<StatementSyntax>.Enumerator en = statements.GetEnumerator();

        if (!en.MoveNext())
            return;

        do
        {
            if (en.Current.IsKind(SyntaxKind.LocalDeclarationStatement))
            {
                var localDeclaration = (LocalDeclarationStatementSyntax)en.Current;

                if (localDeclaration.UsingKeyword.IsKind(SyntaxKind.UsingKeyword))
                    return;
            }
        }
        while (en.MoveNext());

        SyntaxToken openBrace = block.OpenBraceToken;

        if (!AnalyzeTrivia(openBrace.LeadingTrivia))
            return;

        if (!AnalyzeTrivia(openBrace.TrailingTrivia))
            return;

        if (!AnalyzeTrivia(statements[0].GetLeadingTrivia()))
            return;

        if (!AnalyzeTrivia(statements.Last().GetTrailingTrivia()))
            return;

        SyntaxToken closeBrace = block.CloseBraceToken;

        if (!AnalyzeTrivia(closeBrace.LeadingTrivia))
            return;

        if (!AnalyzeTrivia(closeBrace.TrailingTrivia))
            return;

        // If any of the other case blocks contain a definition for the same local variables then removing the brackets would introduce a new error.
        if (switchSection.Parent is SwitchStatementSyntax switchStatement &&
            LocalDeclaredVariablesOverlapWithAnyOtherSwitchSections(switchStatement, switchSection, context.SemanticModel))
            return;

        DiagnosticHelpers.ReportDiagnostic(context, DiagnosticRules.RemoveUnnecessaryBracesInSwitchSection, openBrace);
        DiagnosticHelpers.ReportDiagnostic(context, DiagnosticRules.RemoveUnnecessaryBracesInSwitchSectionFadeOut, closeBrace);

        static bool AnalyzeTrivia(SyntaxTriviaList trivia)
        {
            return trivia.All(f => f.IsKind(SyntaxKind.WhitespaceTrivia, SyntaxKind.EndOfLineTrivia, SyntaxKind.SingleLineCommentTrivia));
        }
    }
    
    private static bool LocalDeclaredVariablesOverlapWithAnyOtherSwitchSections(SwitchStatementSyntax switchStatement, SwitchSectionSyntax switchSection, SemanticModel semanticModel)
    {
        var sectionVariablesDeclared = semanticModel.AnalyzeDataFlow(switchSection)!
            .VariablesDeclared;
        
        if (sectionVariablesDeclared.IsEmpty)
            return false;

        var sectionDeclaredVariablesName = sectionVariablesDeclared
            .Select(s => s.Name)
            .ToImmutableHashSet();

        return switchStatement.Sections.Any(otherSection =>
        {
            var otherSectionVariablesDeclared = semanticModel.AnalyzeDataFlow(otherSection)!.VariablesDeclared;
            if (otherSectionVariablesDeclared.IsEmpty)
                return false;

            return otherSectionVariablesDeclared.Any(s => sectionDeclaredVariablesName.Contains(s.Name));
        });
    }

}
