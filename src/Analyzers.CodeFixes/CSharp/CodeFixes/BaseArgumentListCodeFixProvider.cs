﻿// Copyright (c) Josef Pihrt and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslynator.CodeFixes;
using Roslynator.CSharp.Analysis;

namespace Roslynator.CSharp.CodeFixes
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(BaseArgumentListCodeFixProvider))]
    [Shared]
    public sealed class BaseArgumentListCodeFixProvider : BaseCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(DiagnosticIdentifiers.OrderNamedArguments); }
        }

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            SyntaxNode root = await context.GetSyntaxRootAsync().ConfigureAwait(false);

            if (!TryFindFirstAncestorOrSelf(root, context.Span, out BaseArgumentListSyntax baseArgumentList))
                return;

            if (baseArgumentList.ContainsDirectives(context.Span))
                return;

            Document document = context.Document;
            Diagnostic diagnostic = context.Diagnostics[0];

            switch (diagnostic.Id)
            {
                case DiagnosticIdentifiers.OrderNamedArguments:
                    {
                        CodeAction codeAction = CodeAction.Create(
                            "Order arguments",
                            ct => OrderNamedArgumentsAsync(document, baseArgumentList, ct),
                            GetEquivalenceKey(diagnostic));

                        context.RegisterCodeFix(codeAction, diagnostic);
                        break;
                    }
            }
        }

        private static async Task<Document> OrderNamedArgumentsAsync(
            Document document,
            BaseArgumentListSyntax argumentList,
            CancellationToken cancellationToken)
        {
            SemanticModel semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);

            ImmutableArray<IParameterSymbol> parameters = semanticModel
                .GetSymbol(argumentList.Parent, cancellationToken)
                .ParametersOrDefault();

            SeparatedSyntaxList<ArgumentSyntax> arguments = argumentList.Arguments;

            (int first, int last) = OrderNamedArgumentsAnalyzer.FindFixableSpan(arguments, semanticModel, cancellationToken);

            List<(ArgumentSyntax argument, int ordinal)> sortedArguments = arguments
                .Skip(first)
                .Take(last - first + 1)
                .Select(argument =>
                {
                    IParameterSymbol parameter = parameters.FirstOrDefault(g => g.Name == argument.NameColon.Name.Identifier.ValueText);

                    return (argument, ordinal: parameters.IndexOf(parameter));
                })
                .OrderBy(f => f.ordinal)
                .ToList();

            SeparatedSyntaxList<ArgumentSyntax> newArguments = arguments;

            for (int i = first; i <= last; i++)
            {
                newArguments = newArguments.ReplaceAt(i, sortedArguments[i - first].argument);
            }

            BaseArgumentListSyntax newNode = argumentList
                .WithArguments(newArguments)
                .WithFormatterAnnotation();

            return await document.ReplaceNodeAsync(argumentList, newNode, cancellationToken).ConfigureAwait(false);
        }
    }
}
