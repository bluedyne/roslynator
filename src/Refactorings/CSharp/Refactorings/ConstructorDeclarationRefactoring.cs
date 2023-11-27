﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp.Refactorings;

internal static class ConstructorDeclarationRefactoring
{
    public static async Task ComputeRefactoringsAsync(RefactoringContext context, ConstructorDeclarationSyntax constructorDeclaration)
    {
        if (context.IsRefactoringEnabled(RefactoringDescriptors.ConvertBlockBodyToExpressionBody)
            && context.SupportsCSharp6
            && ConvertBlockBodyToExpressionBodyRefactoring.CanRefactor(constructorDeclaration, context.Span))
        {
            context.RegisterRefactoring(
                ConvertBlockBodyToExpressionBodyRefactoring.Title,
                ct => ConvertBlockBodyToExpressionBodyRefactoring.RefactorAsync(context.Document, constructorDeclaration, ct),
                RefactoringDescriptors.ConvertBlockBodyToExpressionBody);
        }

        if (context.IsRefactoringEnabled(RefactoringDescriptors.CopyDocumentationCommentFromBaseMember)
            && constructorDeclaration.HeaderSpan().Contains(context.Span)
            && !constructorDeclaration.HasDocumentationComment())
        {
            SemanticModel semanticModel = await context.GetSemanticModelAsync().ConfigureAwait(false);

            CopyDocumentationCommentFromBaseMemberRefactoring.ComputeRefactoring(context, constructorDeclaration, semanticModel);
        }
    }
}
