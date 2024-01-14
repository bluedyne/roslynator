﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslynator.CSharp.Analysis;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class UnnecessarySemicolonAtEndOfDeclarationAnalyzer : BaseDiagnosticAnalyzer
{
    private static ImmutableArray<DiagnosticDescriptor> _supportedDiagnostics;

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
    {
        get
        {
            if (_supportedDiagnostics.IsDefault)
                Immutable.InterlockedInitialize(ref _supportedDiagnostics, DiagnosticRules.UnnecessarySemicolonAtEndOfDeclaration);

            return _supportedDiagnostics;
        }
    }

    public override void Initialize(AnalysisContext context)
    {
        base.Initialize(context);

        context.RegisterSyntaxNodeAction(f => AnalyzeNamespaceDeclaration(f), SyntaxKind.NamespaceDeclaration);
        context.RegisterSyntaxNodeAction(f => AnalyzeClassDeclaration(f), SyntaxKind.ClassDeclaration);
        context.RegisterSyntaxNodeAction(f => AnalyzeInterfaceDeclaration(f), SyntaxKind.InterfaceDeclaration);
        context.RegisterSyntaxNodeAction(f => AnalyzeStructDeclaration(f), SyntaxKind.StructDeclaration);
#if ROSLYN_4_0
        context.RegisterSyntaxNodeAction(f => AnalyzeRecordDeclaration(f), SyntaxKind.RecordDeclaration, SyntaxKind.RecordStructDeclaration);
#else
        context.RegisterSyntaxNodeAction(f => AnalyzeRecordDeclaration(f), SyntaxKind.RecordDeclaration);
#endif
        context.RegisterSyntaxNodeAction(f => AnalyzeEnumDeclaration(f), SyntaxKind.EnumDeclaration);
    }

    private static void AnalyzeNamespaceDeclaration(SyntaxNodeAnalysisContext context)
    {
        var declaration = (NamespaceDeclarationSyntax)context.Node;

        SyntaxToken semicolon = declaration.SemicolonToken;

        if (semicolon.Parent is not null
            && !semicolon.IsMissing)
        {
            DiagnosticHelpers.ReportDiagnostic(context, DiagnosticRules.UnnecessarySemicolonAtEndOfDeclaration, semicolon);
        }
    }

    private static void AnalyzeClassDeclaration(SyntaxNodeAnalysisContext context)
    {
        var declaration = (ClassDeclarationSyntax)context.Node;

        if (declaration.CloseBraceToken.IsKind(SyntaxKind.CloseBraceToken))
        {
            SyntaxToken semicolon = declaration.SemicolonToken;

            if (semicolon.Parent is not null
                && !semicolon.IsMissing)
            {
                DiagnosticHelpers.ReportDiagnostic(context, DiagnosticRules.UnnecessarySemicolonAtEndOfDeclaration, semicolon);
            }
        }
    }

    private static void AnalyzeInterfaceDeclaration(SyntaxNodeAnalysisContext context)
    {
        var declaration = (InterfaceDeclarationSyntax)context.Node;

        if (declaration.CloseBraceToken.IsKind(SyntaxKind.CloseBraceToken))
        {
            SyntaxToken semicolon = declaration.SemicolonToken;

            if (semicolon.Parent is not null
                && !semicolon.IsMissing)
            {
                DiagnosticHelpers.ReportDiagnostic(context, DiagnosticRules.UnnecessarySemicolonAtEndOfDeclaration, semicolon);
            }
        }
    }

    private static void AnalyzeStructDeclaration(SyntaxNodeAnalysisContext context)
    {
        var declaration = (StructDeclarationSyntax)context.Node;

        if (declaration.CloseBraceToken.IsKind(SyntaxKind.CloseBraceToken))
        {
            SyntaxToken semicolon = declaration.SemicolonToken;

            if (semicolon.Parent is not null
                && !semicolon.IsMissing)
            {
                DiagnosticHelpers.ReportDiagnostic(context, DiagnosticRules.UnnecessarySemicolonAtEndOfDeclaration, semicolon);
            }
        }
    }

    private static void AnalyzeRecordDeclaration(SyntaxNodeAnalysisContext context)
    {
        var declaration = (RecordDeclarationSyntax)context.Node;

        if (declaration.CloseBraceToken.IsKind(SyntaxKind.CloseBraceToken))
        {
            SyntaxToken semicolon = declaration.SemicolonToken;

            if (semicolon.Parent is not null
                && !semicolon.IsMissing)
            {
                DiagnosticHelpers.ReportDiagnostic(context, DiagnosticRules.UnnecessarySemicolonAtEndOfDeclaration, semicolon);
            }
        }
    }

    private static void AnalyzeEnumDeclaration(SyntaxNodeAnalysisContext context)
    {
        var declaration = (EnumDeclarationSyntax)context.Node;

        if (declaration.CloseBraceToken.IsKind(SyntaxKind.CloseBraceToken))
        {
            SyntaxToken semicolon = declaration.SemicolonToken;

            if (semicolon.Parent is not null
                && !semicolon.IsMissing)
            {
                DiagnosticHelpers.ReportDiagnostic(context, DiagnosticRules.UnnecessarySemicolonAtEndOfDeclaration, semicolon);
            }
        }
    }
}
