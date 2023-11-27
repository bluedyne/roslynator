﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Composition;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Host;
using Roslynator.CSharp;

namespace Roslynator.CSharp;

[Export(typeof(ILanguageService))]
[ExportMetadata("Language", LanguageNames.CSharp)]
[ExportMetadata("ServiceType", "Roslynator.ISyntaxFactsService")]
internal sealed class CSharpSyntaxFactsService : ISyntaxFactsService
{
    public static CSharpSyntaxFactsService Instance { get; } = new();

    public string SingleLineCommentStart => "//";

    public bool IsEndOfLineTrivia(SyntaxTrivia trivia)
    {
        return trivia.IsKind(SyntaxKind.EndOfLineTrivia);
    }

    public bool IsComment(SyntaxTrivia trivia)
    {
        return trivia.IsKind(SyntaxKind.SingleLineCommentTrivia, SyntaxKind.MultiLineCommentTrivia);
    }

    public bool IsSingleLineComment(SyntaxTrivia trivia)
    {
        return trivia.IsKind(SyntaxKind.SingleLineCommentTrivia);
    }

    public bool IsWhitespaceTrivia(SyntaxTrivia trivia)
    {
        return trivia.IsKind(SyntaxKind.WhitespaceTrivia);
    }

    public SyntaxTriviaList ParseLeadingTrivia(string text, int offset = 0)
    {
        return SyntaxFactory.ParseLeadingTrivia(text, offset);
    }

    public SyntaxTriviaList ParseTrailingTrivia(string text, int offset = 0)
    {
        return SyntaxFactory.ParseTrailingTrivia(text, offset);
    }

    public bool BeginsWithAutoGeneratedComment(SyntaxNode root)
    {
        return GeneratedCodeUtility.BeginsWithAutoGeneratedComment(
            root,
            f => f.IsKind(SyntaxKind.SingleLineCommentTrivia, SyntaxKind.MultiLineCommentTrivia));
    }

    public bool AreEquivalent(SyntaxTree oldTree, SyntaxTree newTree)
    {
        return SyntaxFactory.AreEquivalent(oldTree, newTree, topLevel: false);
    }

    public SyntaxNode? GetSymbolDeclaration(SyntaxToken identifier)
    {
        SyntaxNode? parent = identifier.Parent;

        if (!identifier.IsKind(SyntaxKind.IdentifierToken))
            return null;

        if (parent is null)
            return null;

        switch (parent.Kind())
        {
            case SyntaxKind.TupleElement:
            case SyntaxKind.LocalFunctionStatement:
            case SyntaxKind.VariableDeclarator:
            case SyntaxKind.SingleVariableDesignation:
            case SyntaxKind.CatchDeclaration:
            case SyntaxKind.TypeParameter:
            case SyntaxKind.ClassDeclaration:
            case SyntaxKind.StructDeclaration:
            case SyntaxKind.RecordStructDeclaration:
            case SyntaxKind.InterfaceDeclaration:
            case SyntaxKind.RecordDeclaration:
            case SyntaxKind.EnumDeclaration:
            case SyntaxKind.DelegateDeclaration:
            case SyntaxKind.EnumMemberDeclaration:
            case SyntaxKind.MethodDeclaration:
            case SyntaxKind.PropertyDeclaration:
            case SyntaxKind.EventDeclaration:
            case SyntaxKind.Parameter:
            case SyntaxKind.ForEachStatement:
                {
                    return parent;
                }
            case SyntaxKind.IdentifierName:
                {
                    parent = parent.Parent;

                    if (parent.IsKind(SyntaxKind.NameEquals))
                    {
                        parent = parent.Parent;

                        if (parent.IsKind(
                            SyntaxKind.UsingDirective,
                            SyntaxKind.AnonymousObjectMemberDeclarator))
                        {
                            return parent;
                        }

                        SyntaxDebug.Fail(parent);

                        return null;
                    }

                    return parent;
                }
        }

        SyntaxDebug.Fail(parent);
        return null;
    }

    public bool IsValidIdentifier(string name)
    {
        return SyntaxFacts.IsValidIdentifier(name);
    }
}
