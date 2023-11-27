﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp.Refactorings;

internal static class VariableDeclaratorRefactoring
{
    public static void ComputeRefactorings(RefactoringContext context, VariableDeclaratorSyntax variableDeclarator)
    {
        if (context.IsRefactoringEnabled(RefactoringDescriptors.InitializeFieldFromConstructor)
            && context.Span.IsEmptyAndContainedInSpanOrBetweenSpans(variableDeclarator.Identifier))
        {
            InitializeFieldFromConstructorRefactoring.ComputeRefactoring(context, variableDeclarator);
        }
    }
}
