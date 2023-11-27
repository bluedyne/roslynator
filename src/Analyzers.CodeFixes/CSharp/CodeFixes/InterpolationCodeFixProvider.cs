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

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(InterpolationCodeFixProvider))]
[Shared]
public sealed class InterpolationCodeFixProvider : BaseCodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds
    {
        get { return ImmutableArray.Create(DiagnosticIdentifiers.UnnecessaryInterpolation); }
    }

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        SyntaxNode root = await context.GetSyntaxRootAsync().ConfigureAwait(false);

        if (!TryFindFirstAncestorOrSelf(root, context.Span, out InterpolationSyntax interpolation))
            return;

        Diagnostic diagnostic = context.Diagnostics[0];

        CodeAction codeAction = CodeAction.Create(
            "Make string literal part of interpolated string",
            ct => UnnecessaryInterpolationRefactoring.RefactorAsync(context.Document, interpolation, ct),
            GetEquivalenceKey(diagnostic));

        context.RegisterCodeFix(codeAction, diagnostic);
    }
}
