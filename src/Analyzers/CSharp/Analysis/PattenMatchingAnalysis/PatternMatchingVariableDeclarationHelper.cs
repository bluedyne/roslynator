// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp.Analysis;

public static class PatternMatchingVariableDeclarationHelper
{
    public static bool AnyDeclaredVariablesMatch(PatternSyntax pattern, ImmutableHashSet<string> variableNames)
    {
        return pattern switch
        {
            RecursivePatternSyntax { PositionalPatternClause: var positionalPatternClause, PropertyPatternClause: var propertyPatternClause, Designation: var designation } =>
                (designation is not null && AnyDeclaredVariablesMatch(designation, variableNames))
                    || (propertyPatternClause?.Subpatterns.Any(p => AnyDeclaredVariablesMatch(p.Pattern, variableNames)) ?? false)
                    || (positionalPatternClause?.Subpatterns.Any(p => AnyDeclaredVariablesMatch(p.Pattern, variableNames)) ?? false),
            BinaryPatternSyntax binaryPattern =>
                AnyDeclaredVariablesMatch(binaryPattern.Left, variableNames)
                    || AnyDeclaredVariablesMatch(binaryPattern.Right, variableNames),
            ParenthesizedPatternSyntax parenthesizedPattern => AnyDeclaredVariablesMatch(parenthesizedPattern.Pattern, variableNames),
            DeclarationPatternSyntax { Designation: var variableDesignation } => AnyDeclaredVariablesMatch(variableDesignation, variableNames),
            VarPatternSyntax { Designation: var variableDesignation } => AnyDeclaredVariablesMatch(variableDesignation, variableNames),
            _ => false
        };
    }

    internal static bool AnyDeclaredVariablesMatch(VariableDesignationSyntax designation, ImmutableHashSet<string> variableNames)
    {
        return designation switch
        {
            SingleVariableDesignationSyntax singleVariableDesignation => variableNames.Contains(singleVariableDesignation.Identifier.ValueText),
            ParenthesizedVariableDesignationSyntax parenthesizedVariableDesignation => parenthesizedVariableDesignation.Variables.Any(variable => AnyDeclaredVariablesMatch(variable, variableNames)),
            DiscardDesignationSyntax _ => false,
            _ => false
        };
    }
}
