﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslynator.CodeFixes;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Roslynator.CSharp.CSharpFactory;

namespace Roslynator.CSharp.CodeFixes;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CompositeEnumValueContainsUndefinedFlagCodeFixProvider))]
[Shared]
public sealed class CompositeEnumValueContainsUndefinedFlagCodeFixProvider : BaseCodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds
    {
        get { return ImmutableArray.Create(DiagnosticIdentifiers.CompositeEnumValueContainsUndefinedFlag); }
    }

    public override FixAllProvider GetFixAllProvider() => null;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        SyntaxNode root = await context.GetSyntaxRootAsync().ConfigureAwait(false);

        if (!TryFindFirstAncestorOrSelf(root, context.Span, out EnumDeclarationSyntax enumDeclaration))
            return;

        Diagnostic diagnostic = context.Diagnostics[0];

        string value = diagnostic.Properties["Value"];

        CodeAction codeAction = CodeAction.Create(
            $"Declare enum member with value {value}",
            ct => RefactorAsync(context.Document, enumDeclaration, value, ct),
            GetEquivalenceKey(diagnostic));

        context.RegisterCodeFix(codeAction, diagnostic);
    }

    private static async Task<Document> RefactorAsync(
        Document document,
        EnumDeclarationSyntax enumDeclaration,
        string value,
        CancellationToken cancellationToken)
    {
        SemanticModel semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);

        INamedTypeSymbol symbol = semanticModel.GetDeclaredSymbol(enumDeclaration, cancellationToken);

        string name = NameGenerator.Default.EnsureUniqueEnumMemberName(DefaultNames.EnumMember, symbol);

        EnumMemberDeclarationSyntax enumMember = EnumMemberDeclaration(
            Identifier(name).WithRenameAnnotation(),
            ParseExpression(value));

        enumMember = enumMember.WithTrailingTrivia(NewLine());

        EnumDeclarationSyntax newNode = enumDeclaration.WithMembers(enumDeclaration.Members.Add(enumMember));

        return await document.ReplaceNodeAsync(enumDeclaration, newNode, cancellationToken).ConfigureAwait(false);
    }
}
