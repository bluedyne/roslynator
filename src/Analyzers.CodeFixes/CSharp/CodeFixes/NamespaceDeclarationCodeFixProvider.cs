﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Composition;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslynator.CodeFixes;
using Roslynator.CSharp.Refactorings;

namespace Roslynator.CSharp.CodeFixes;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(NamespaceDeclarationCodeFixProvider))]
[Shared]
public sealed class NamespaceDeclarationCodeFixProvider : BaseCodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds
    {
        get
        {
            return ImmutableArray.Create(
                DiagnosticIdentifiers.RemoveEmptyNamespaceDeclaration,
                DiagnosticIdentifiers.DeclareUsingDirectiveOnTopLevel);
        }
    }

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        SyntaxNode root = await context.GetSyntaxRootAsync().ConfigureAwait(false);

        if (!TryFindFirstAncestorOrSelf(root, context.Span, out NamespaceDeclarationSyntax namespaceDeclaration))
            return;

        foreach (Diagnostic diagnostic in context.Diagnostics)
        {
            switch (diagnostic.Id)
            {
                case DiagnosticIdentifiers.RemoveEmptyNamespaceDeclaration:
                    {
                        CodeAction codeAction = CodeAction.Create(
                            "Remove empty namespace declaration",
                            ct => context.Document.RemoveNodeAsync(namespaceDeclaration, ct),
                            GetEquivalenceKey(diagnostic));

                        context.RegisterCodeFix(codeAction, diagnostic);
                        break;
                    }
                case DiagnosticIdentifiers.DeclareUsingDirectiveOnTopLevel:
                    {
                        string title = (namespaceDeclaration.Usings.Count == 1)
                            ? "Move using to top level"
                            : "Move usings to top level";

                        CodeAction codeAction = CodeAction.Create(
                            title,
                            ct => DeclareUsingDirectiveOnTopLevelRefactoring.RefactorAsync(context.Document, namespaceDeclaration, ct),
                            GetEquivalenceKey(diagnostic));

                        context.RegisterCodeFix(codeAction, diagnostic);
                        break;
                    }
            }
        }
    }
}
