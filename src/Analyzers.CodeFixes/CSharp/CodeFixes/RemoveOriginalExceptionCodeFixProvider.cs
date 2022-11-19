﻿// Copyright (c) Josef Pihrt and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(RemoveOriginalExceptionCodeFixProvider))]
[Shared]
public sealed class RemoveOriginalExceptionCodeFixProvider : BaseCodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds
    {
        get { return ImmutableArray.Create(DiagnosticIdentifiers.RemoveOriginalExceptionFromThrowStatement); }
    }

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        SyntaxNode root = await context.GetSyntaxRootAsync().ConfigureAwait(false);

        if (!TryFindFirstAncestorOrSelf(root, context.Span, out ThrowStatementSyntax throwStatement))
            return;

        CodeAction codeAction = CodeAction.Create(
            "Remove original exception from throw statement",
            ct => RemoveOriginalExceptionFromThrowStatementRefactoring.RefactorAsync(context.Document, throwStatement, ct),
            GetEquivalenceKey(DiagnosticIdentifiers.RemoveOriginalExceptionFromThrowStatement));

        context.RegisterCodeFix(codeAction, context.Diagnostics);
    }
}
