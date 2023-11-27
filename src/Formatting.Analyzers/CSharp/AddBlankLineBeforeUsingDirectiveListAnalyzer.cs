﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using Roslynator.CSharp;

namespace Roslynator.Formatting.CSharp;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class AddBlankLineBeforeUsingDirectiveListAnalyzer : BaseDiagnosticAnalyzer
{
    private static ImmutableArray<DiagnosticDescriptor> _supportedDiagnostics;

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
    {
        get
        {
            if (_supportedDiagnostics.IsDefault)
                Immutable.InterlockedInitialize(ref _supportedDiagnostics, DiagnosticRules.AddBlankLineBeforeUsingDirectiveList);

            return _supportedDiagnostics;
        }
    }

    public override void Initialize(AnalysisContext context)
    {
        base.Initialize(context);

        context.RegisterSyntaxNodeAction(f => AnalyzeCompilationUnit(f), SyntaxKind.CompilationUnit);
    }

    private static void AnalyzeCompilationUnit(SyntaxNodeAnalysisContext context)
    {
        var compilationUnit = (CompilationUnitSyntax)context.Node;

        UsingDirectiveSyntax usingDirective = compilationUnit.Usings.FirstOrDefault();

        if (usingDirective is null)
            return;

        SyntaxTriviaList.Reversed.Enumerator en = usingDirective.GetLeadingTrivia().Reverse().GetEnumerator();

        if (en.MoveNext())
        {
            if (en.Current.IsWhitespaceTrivia()
                && !en.MoveNext())
            {
                if (IsPrecededWithExternAliasDirective())
                    ReportDiagnostic(usingDirective.SpanStart);
            }
            else if (en.Current.IsEndOfLineTrivia()
                && en.MoveNext()
                && en.Current.IsKind(SyntaxKind.SingleLineCommentTrivia))
            {
                ReportDiagnostic(usingDirective.SpanStart);
            }
        }
        else if (IsPrecededWithExternAliasDirective())
        {
            ReportDiagnostic(usingDirective.SpanStart);
        }

        bool IsPrecededWithExternAliasDirective()
        {
            ExternAliasDirectiveSyntax externAliasDirective = compilationUnit.Externs.LastOrDefault();

            return externAliasDirective?.FullSpan.End == usingDirective.FullSpan.Start
                && SyntaxTriviaAnalysis.IsOptionalWhitespaceThenOptionalSingleLineCommentThenEndOfLineTrivia(externAliasDirective.GetTrailingTrivia());
        }

        void ReportDiagnostic(int position)
        {
            DiagnosticHelpers.ReportDiagnostic(
                context,
                DiagnosticRules.AddBlankLineBeforeUsingDirectiveList,
                Location.Create(compilationUnit.SyntaxTree, new TextSpan(position, 0)));
        }
    }
}
