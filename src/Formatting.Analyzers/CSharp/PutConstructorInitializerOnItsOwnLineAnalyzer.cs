﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp;

namespace Roslynator.Formatting.CSharp;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class PutConstructorInitializerOnItsOwnLineAnalyzer : BaseDiagnosticAnalyzer
{
    private static ImmutableArray<DiagnosticDescriptor> _supportedDiagnostics;

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
    {
        get
        {
            if (_supportedDiagnostics.IsDefault)
                Immutable.InterlockedInitialize(ref _supportedDiagnostics, DiagnosticRules.PutConstructorInitializerOnItsOwnLine);

            return _supportedDiagnostics;
        }
    }

    public override void Initialize(AnalysisContext context)
    {
        base.Initialize(context);

        context.RegisterSyntaxNodeAction(f => AnalyzeConstructorInitializer(f), SyntaxKind.ThisConstructorInitializer);
        context.RegisterSyntaxNodeAction(f => AnalyzeConstructorInitializer(f), SyntaxKind.BaseConstructorInitializer);
    }

    private static void AnalyzeConstructorInitializer(SyntaxNodeAnalysisContext context)
    {
        var constructorInitializer = (ConstructorInitializerSyntax)context.Node;

        SyntaxToken colonToken = constructorInitializer.ColonToken;

        TriviaBlock block = TriviaBlock.FromBetween(colonToken.GetPreviousToken(), colonToken);

        if (block.Kind == TriviaBlockKind.NoNewLine)
        {
            DiagnosticHelpers.ReportDiagnostic(
                context,
                DiagnosticRules.PutConstructorInitializerOnItsOwnLine,
                block.GetLocation());
        }
    }
}
