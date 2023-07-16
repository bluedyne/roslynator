﻿// Copyright (c) Josef Pihrt and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslynator.CSharp.Analysis;
using Roslynator.CSharp.Syntax;

namespace Roslynator.CSharp.Refactorings;

internal static class UseElementAccessRefactoring
{
    public static async Task ComputeRefactoringsAsync(RefactoringContext context, InvocationExpressionSyntax invocation)
    {
        if (invocation.IsParentKind(SyntaxKind.ExpressionStatement))
            return;

        SimpleMemberInvocationExpressionInfo invocationInfo = SyntaxInfo.SimpleMemberInvocationExpressionInfo(invocation);

        if (!invocationInfo.Success)
            return;

        switch (invocationInfo.NameText)
        {
            case "First":
                {
                    if (invocationInfo.Arguments.Any())
                        break;

                    SemanticModel semanticModel = await context.GetSemanticModelAsync().ConfigureAwait(false);

                    if (!UseElementAccessAnalysis.IsFixableFirst(invocationInfo, semanticModel, context.CancellationToken))
                        break;

                    context.RegisterRefactoring(
                        "Use [] instead of calling 'First'",
                        ct => UseElementAccessInsteadOfEnumerableMethodRefactoring.UseElementAccessInsteadOfFirstAsync(context.Document, invocation, ct),
                        RefactoringDescriptors.UseElementAccessInsteadOfLinqMethod);

                    break;
                }
            case "Last":
                {
                    if (invocationInfo.Arguments.Any())
                        break;

                    SemanticModel semanticModel = await context.GetSemanticModelAsync().ConfigureAwait(false);

                    if (!UseElementAccessAnalysis.IsFixableLast(invocationInfo, semanticModel, context.CancellationToken))
                        break;

                    string propertyName = CSharpUtility.GetCountOrLengthPropertyName(invocationInfo.Expression, semanticModel, context.CancellationToken);

                    if (propertyName is null)
                        break;

                    context.RegisterRefactoring(
                        "Use [] instead of calling 'Last'",
                        ct => UseElementAccessInsteadOfEnumerableMethodRefactoring.UseElementAccessInsteadOfLastAsync(context.Document, invocation, propertyName, ct),
                        RefactoringDescriptors.UseElementAccessInsteadOfLinqMethod);

                    break;
                }
            case "ElementAt":
                {
                    if (invocationInfo.Arguments.SingleOrDefault(shouldThrow: false)?.Expression?.IsMissing != false)
                        break;

                    SemanticModel semanticModel = await context.GetSemanticModelAsync().ConfigureAwait(false);

                    if (!UseElementAccessAnalysis.IsFixableElementAt(invocationInfo, semanticModel, context.CancellationToken))
                        break;

                    context.RegisterRefactoring(
                        "Use [] instead of calling 'ElementAt'",
                        ct => UseElementAccessInsteadOfEnumerableMethodRefactoring.UseElementAccessInsteadOfElementAtAsync(context.Document, invocation, ct),
                        RefactoringDescriptors.UseElementAccessInsteadOfLinqMethod);

                    break;
                }
        }
    }
}
