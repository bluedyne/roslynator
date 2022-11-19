﻿// Copyright (c) Josef Pihrt and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace Roslynator.CSharp.Analysis;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class AsyncSuffixAnalyzer : BaseDiagnosticAnalyzer
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
                    DiagnosticRules.AsynchronousMethodNameShouldEndWithAsync,
                    DiagnosticRules.NonAsynchronousMethodNameShouldNotEndWithAsync,
                    DiagnosticRules.NonAsynchronousMethodNameShouldNotEndWithAsyncFadeOut);
            }

            return _supportedDiagnostics;
        }
    }

    public override void Initialize(AnalysisContext context)
    {
        base.Initialize(context);

        context.RegisterCompilationStartAction(startContext =>
        {
            INamedTypeSymbol asyncAction = startContext.Compilation.GetTypeByMetadataName("Windows.Foundation.IAsyncAction");

            bool shouldCheckWindowsRuntimeTypes = asyncAction is not null;

            startContext.RegisterSyntaxNodeAction(
                c =>
                {
                    if (DiagnosticHelpers.IsAnyEffective(
                        c,
                        DiagnosticRules.AsynchronousMethodNameShouldEndWithAsync,
                        DiagnosticRules.NonAsynchronousMethodNameShouldNotEndWithAsync))
                    {
                        AnalyzeMethodDeclaration(c, shouldCheckWindowsRuntimeTypes);
                    }
                },
                SyntaxKind.MethodDeclaration);
        });
    }

    private static void AnalyzeMethodDeclaration(SyntaxNodeAnalysisContext context, bool shouldCheckWindowsRuntimeTypes)
    {
        var methodDeclaration = (MethodDeclarationSyntax)context.Node;

        if (methodDeclaration.Modifiers.Contains(SyntaxKind.OverrideKeyword))
            return;

        if (methodDeclaration.Identifier.ValueText.EndsWith("Async", StringComparison.Ordinal))
        {
            IMethodSymbol methodSymbol = context.SemanticModel.GetDeclaredSymbol(methodDeclaration, context.CancellationToken);

            if (methodSymbol.IsAsync)
                return;

            if (!methodSymbol.Name.EndsWith("Async", StringComparison.Ordinal))
                return;

            if (SymbolUtility.IsAwaitable(methodSymbol.ReturnType, shouldCheckWindowsRuntimeTypes)
                || methodSymbol.ReturnType.OriginalDefinition.HasMetadataName(MetadataNames.System_Collections_Generic_IAsyncEnumerable_T))
            {
                return;
            }

            SyntaxToken identifier = methodDeclaration.Identifier;

            DiagnosticHelpers.ReportDiagnostic(
                context,
                DiagnosticRules.NonAsynchronousMethodNameShouldNotEndWithAsync,
                identifier);

            DiagnosticHelpers.ReportDiagnostic(
                context,
                DiagnosticRules.NonAsynchronousMethodNameShouldNotEndWithAsyncFadeOut,
                Location.Create(identifier.SyntaxTree, TextSpan.FromBounds(identifier.Span.End - 5, identifier.Span.End)));
        }
        else
        {
            IMethodSymbol methodSymbol = context.SemanticModel.GetDeclaredSymbol(methodDeclaration, context.CancellationToken);

            if (methodSymbol.Name.EndsWith("Async", StringComparison.Ordinal))
                return;

            if (SymbolUtility.CanBeEntryPoint(methodSymbol))
                return;

            if (!SymbolUtility.IsAwaitable(methodSymbol.ReturnType, shouldCheckWindowsRuntimeTypes)
                && !methodSymbol.ReturnType.OriginalDefinition.HasMetadataName(MetadataNames.System_Collections_Generic_IAsyncEnumerable_T))
            {
                return;
            }

            DiagnosticHelpers.ReportDiagnostic(context, DiagnosticRules.AsynchronousMethodNameShouldEndWithAsync, methodDeclaration.Identifier);
        }
    }
}
