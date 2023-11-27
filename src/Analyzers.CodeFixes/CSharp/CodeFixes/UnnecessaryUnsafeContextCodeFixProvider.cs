﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Composition;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslynator.CodeFixes;

namespace Roslynator.CSharp.CodeFixes;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(UnnecessaryUnsafeContextCodeFixProvider))]
[Shared]
public sealed class UnnecessaryUnsafeContextCodeFixProvider : BaseCodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds
    {
        get { return ImmutableArray.Create(DiagnosticIdentifiers.UnnecessaryUnsafeContext); }
    }

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        SyntaxNode root = await context.GetSyntaxRootAsync().ConfigureAwait(false);

        if (!TryFindToken(root, context.Span.Start, out SyntaxToken token))
            return;

        Debug.Assert(token.IsKind(SyntaxKind.UnsafeKeyword), token.Kind().ToString());

        if (!token.IsKind(SyntaxKind.UnsafeKeyword))
            return;

        Diagnostic diagnostic = context.Diagnostics[0];

        SyntaxNode parent = token.Parent;

        if (parent is UnsafeStatementSyntax unsafeStatement)
        {
            CodeAction codeAction = CodeAction.Create(
                "Remove unsafe context",
                ct => context.Document.ReplaceNodeAsync(unsafeStatement, SyntaxRefactorings.RemoveUnsafeContext(unsafeStatement), ct),
                GetEquivalenceKey(diagnostic));

            context.RegisterCodeFix(codeAction, diagnostic);
        }
        else
        {
            ModifiersCodeFixRegistrator.RemoveModifier(context, diagnostic, parent, token);
        }
    }
}
