﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Roslynator.CSharp.Refactorings.AddOrRemoveParameterName;

internal class AddParameterNameRewriter : CSharpSyntaxRewriter
{
    private static readonly SymbolDisplayFormat _symbolDisplayFormat = new(
        parameterOptions: SymbolDisplayParameterOptions.IncludeName,
        miscellaneousOptions: SymbolDisplayMiscellaneousOptions.EscapeKeywordIdentifiers);

    private readonly ImmutableArray<ArgumentSyntax> _arguments;
    private readonly SemanticModel _semanticModel;

    public AddParameterNameRewriter(ImmutableArray<ArgumentSyntax> arguments, SemanticModel semanticModel)
    {
        _arguments = arguments;
        _semanticModel = semanticModel;
    }

    public override SyntaxNode VisitArgument(ArgumentSyntax node)
    {
        if (_arguments.Contains(node))
        {
            return AddParameterName(node, _semanticModel);
        }
        else
        {
            return base.VisitArgument(node);
        }
    }

    private static ArgumentSyntax AddParameterName(
        ArgumentSyntax argument,
        SemanticModel semanticModel,
        CancellationToken cancellationToken = default)
    {
        if (argument.NameColon?.IsMissing != false)
        {
            IParameterSymbol parameterSymbol = semanticModel.DetermineParameter(
                argument,
                allowParams: false,
                cancellationToken: cancellationToken);

            if (parameterSymbol is not null)
            {
                ArgumentSyntax newArgument = argument.WithoutLeadingTrivia();

                return newArgument
                    .WithNameColon(
                        NameColon(parameterSymbol.ToDisplayString(_symbolDisplayFormat))
                            .WithTrailingTrivia(Space))
                    .WithLeadingTrivia(argument.GetLeadingTrivia());
            }
        }

        return argument;
    }
}
