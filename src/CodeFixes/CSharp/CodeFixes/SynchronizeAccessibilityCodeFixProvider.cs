﻿// Copyright (c) Josef Pihrt and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslynator.CodeFixes;
using Roslynator.CSharp.Refactorings;

namespace Roslynator.CSharp.CodeFixes;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(SynchronizeAccessibilityCodeFixProvider))]
[Shared]
public sealed class SynchronizeAccessibilityCodeFixProvider : CompilerDiagnosticCodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds
    {
        get { return ImmutableArray.Create(CompilerDiagnosticIdentifiers.CS0262_PartialDeclarationsHaveConflictingAccessibilityModifiers); }
    }

    public override FixAllProvider GetFixAllProvider()
    {
        return null;
    }

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        Diagnostic diagnostic = context.Diagnostics[0];

        SyntaxNode root = await context.GetSyntaxRootAsync().ConfigureAwait(false);

        if (!IsEnabled(diagnostic.Id, CodeFixIdentifiers.SynchronizeAccessibility, context.Document, root.SyntaxTree))
            return;

        if (!TryFindFirstAncestorOrSelf(root, context.Span, out MemberDeclarationSyntax memberDeclaration))
            return;

        SemanticModel semanticModel = await context.GetSemanticModelAsync().ConfigureAwait(false);

        var symbol = (INamedTypeSymbol)semanticModel.GetDeclaredSymbol(memberDeclaration, context.CancellationToken);

        ImmutableArray<MemberDeclarationSyntax> memberDeclarations = ImmutableArray.CreateRange(
            symbol.DeclaringSyntaxReferences,
            f => (MemberDeclarationSyntax)f.GetSyntax(context.CancellationToken));

        foreach (Accessibility accessibility in memberDeclarations
            .Select(f => SyntaxAccessibility.GetExplicitAccessibility(f))
            .Where(f => f != Accessibility.NotApplicable))
        {
            if (SyntaxAccessibility.IsValidAccessibility(memberDeclaration, accessibility))
            {
                CodeAction codeAction = CodeAction.Create(
                    $"Change accessibility to '{SyntaxFacts.GetText(accessibility)}'",
                    ct => ChangeAccessibilityRefactoring.RefactorAsync(context.Solution(), memberDeclarations, accessibility, ct),
                    GetEquivalenceKey(CompilerDiagnosticIdentifiers.CS0262_PartialDeclarationsHaveConflictingAccessibilityModifiers, accessibility.ToString()));

                context.RegisterCodeFix(codeAction, diagnostic);
            }
        }
    }
}
