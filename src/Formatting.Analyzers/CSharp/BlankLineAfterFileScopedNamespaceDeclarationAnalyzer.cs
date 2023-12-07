﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp;
using Roslynator.CSharp.CodeStyle;

namespace Roslynator.Formatting.CSharp;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class BlankLineAfterFileScopedNamespaceDeclarationAnalyzer : BaseDiagnosticAnalyzer
{
    private static ImmutableArray<DiagnosticDescriptor> _supportedDiagnostics;

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
    {
        get
        {
            if (_supportedDiagnostics.IsDefault)
                Immutable.InterlockedInitialize(ref _supportedDiagnostics, DiagnosticRules.BlankLineAfterFileScopedNamespaceDeclaration);

            return _supportedDiagnostics;
        }
    }

    public override void Initialize(AnalysisContext context)
    {
        base.Initialize(context);

        context.RegisterSyntaxNodeAction(f => AnalyzeFileScopedNamespaceDeclaration(f), SyntaxKind.FileScopedNamespaceDeclaration);
    }

    private static void AnalyzeFileScopedNamespaceDeclaration(SyntaxNodeAnalysisContext context)
    {
        var namespaceDeclaration = (FileScopedNamespaceDeclarationSyntax)context.Node;

        SyntaxNode node = GetNodeAfterNamespaceDeclaration(namespaceDeclaration);

        if (node is null)
            return;

        BlankLineStyle style = context.GetBlankLineAfterFileScopedNamespaceDeclaration();

        if (style == BlankLineStyle.None)
            return;

        SyntaxToken semicolon = namespaceDeclaration.SemicolonToken;

        if (semicolon.IsMissing)
            return;

        TriviaBlock block = TriviaBlock.FromBetween(semicolon, node);

        if (!block.Success)
            return;

        if (block.Kind == TriviaBlockKind.BlankLine)
        {
            if (style == BlankLineStyle.Remove)
            {
                context.ReportDiagnostic(
                    DiagnosticRules.BlankLineAfterFileScopedNamespaceDeclaration,
                    block.GetLocation(),
                    "Remove");
            }
        }
        else if (style == BlankLineStyle.Add)
        {
            context.ReportDiagnostic(
                DiagnosticRules.BlankLineAfterFileScopedNamespaceDeclaration,
                block.GetLocation(),
                "Add");
        }
    }

    internal static SyntaxNode GetNodeAfterNamespaceDeclaration(FileScopedNamespaceDeclarationSyntax namespaceDeclaration)
    {
        MemberDeclarationSyntax memberDeclaration = namespaceDeclaration.Members.FirstOrDefault();
        UsingDirectiveSyntax usingDirective = namespaceDeclaration.Usings.FirstOrDefault();

        return (usingDirective?.SpanStart > namespaceDeclaration.SpanStart)
            ? usingDirective
            : memberDeclaration;
    }
}
