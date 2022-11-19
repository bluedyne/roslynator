﻿// Copyright (c) Josef Pihrt and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslynator.CSharp.Analysis;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class RemoveRedundantConstructorAnalyzer : BaseDiagnosticAnalyzer
{
    private static readonly MetadataName _usedImplicitlyAttribute = MetadataName.Parse("JetBrains.Annotations.UsedImplicitlyAttribute");

    private static ImmutableArray<DiagnosticDescriptor> _supportedDiagnostics;

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
    {
        get
        {
            if (_supportedDiagnostics.IsDefault)
                Immutable.InterlockedInitialize(ref _supportedDiagnostics, DiagnosticRules.RemoveRedundantConstructor);

            return _supportedDiagnostics;
        }
    }

    public override void Initialize(AnalysisContext context)
    {
        base.Initialize(context);

        context.RegisterSyntaxNodeAction(f => AnalyzeConstructorDeclaration(f), SyntaxKind.ConstructorDeclaration);
    }

    private static void AnalyzeConstructorDeclaration(SyntaxNodeAnalysisContext context)
    {
        var constructor = (ConstructorDeclarationSyntax)context.Node;

        if (constructor.ContainsDiagnostics)
            return;

        if (constructor.ParameterList?.Parameters.Any() != false)
            return;

        if (constructor.Body?.Statements.Any() != false)
            return;

        SyntaxTokenList modifiers = constructor.Modifiers;

        if (!modifiers.Contains(SyntaxKind.PublicKeyword))
            return;

        if (modifiers.Contains(SyntaxKind.StaticKeyword))
            return;

        ConstructorInitializerSyntax initializer = constructor.Initializer;

        if (initializer != null
            && initializer.ArgumentList?.Arguments.Any() != false)
        {
            return;
        }

        if (constructor.HasDocumentationComment())
            return;

        IMethodSymbol symbol = context.SemanticModel.GetDeclaredSymbol(constructor, context.CancellationToken);

        if (symbol?.Kind != SymbolKind.Method)
            return;

        if (symbol.ContainingType.InstanceConstructors.SingleOrDefault(shouldThrow: false) != symbol)
            return;

        if (symbol.HasAttribute(_usedImplicitlyAttribute))
            return;

        if (!constructor.DescendantTrivia(constructor.Span).All(f => f.IsWhitespaceOrEndOfLineTrivia()))
            return;

        DiagnosticHelpers.ReportDiagnostic(context, DiagnosticRules.RemoveRedundantConstructor, constructor);
    }
}
