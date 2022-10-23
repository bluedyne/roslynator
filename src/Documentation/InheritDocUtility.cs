﻿// Copyright (c) Josef Pihrt and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;

namespace Roslynator.Documentation
{
    internal static class InheritDocUtility
    {
        public static XElement FindInheritedDocumentation(ISymbol symbol, Func<ISymbol, XElement> getDocumentation)
        {
            if (symbol.IsKind(SymbolKind.Method, SymbolKind.Property, SymbolKind.Event))
            {
                if (symbol is IMethodSymbol methodSymbol
                    && methodSymbol.MethodKind == MethodKind.Constructor)
                {
                    return FindInheritedDocumentationCommentFromBaseConstructor(methodSymbol, getDocumentation);
                }

                return FindInheritedDocumentationFromBaseMember(symbol, getDocumentation)
                    ?? FindInheritedDocumentationFromImplementedInterfaceMember(symbol, getDocumentation);
            }
            else if (symbol is INamedTypeSymbol namedTypeSymbol)
            {
                return FindInheritedDocumentationFromBaseType(namedTypeSymbol, getDocumentation)
                    ?? FindInheritedDocumentationFromImplementedInterface(namedTypeSymbol, getDocumentation);
            }

            return null;
        }

        public static XElement FindInheritedDocumentationCommentFromBaseConstructor(IMethodSymbol symbol, Func<ISymbol, XElement> getDocumentation)
        {
            foreach (INamedTypeSymbol baseType in symbol.ContainingType.BaseTypes())
            {
                foreach (IMethodSymbol baseConstructor in baseType.Constructors)
                {
                    if (ParametersEqual(symbol, baseConstructor))
                    {
                        XElement element = getDocumentation(baseConstructor);

                        if (!ContainsInheritDoc(element))
                            return element;
                    }
                }
            }

            return null;

            static bool ParametersEqual(IMethodSymbol x, IMethodSymbol y)
            {
                ImmutableArray<IParameterSymbol> parameters1 = x.Parameters;
                ImmutableArray<IParameterSymbol> parameters2 = y.Parameters;

                if (parameters1.Length != parameters2.Length)
                    return false;

                for (int i = 0; i < parameters1.Length; i++)
                {
                    if (!SymbolEqualityComparer.Default.Equals(parameters1[i].Type, parameters2[i].Type))
                        return false;
                }

                return true;
            }
        }

        public static XElement FindInheritedDocumentationFromBaseMember(ISymbol symbol, Func<ISymbol, XElement> getDocumentation)
        {
            ISymbol s = symbol;

            while ((s = s.OverriddenSymbol()) is not null)
            {
                XElement element = getDocumentation(s);

                if (!ContainsInheritDoc(element))
                    return element;
            }

            return null;
        }

        public static XElement FindInheritedDocumentationFromImplementedInterfaceMember(ISymbol symbol, Func<ISymbol, XElement> getDocumentation)
        {
            INamedTypeSymbol containingType = symbol.ContainingType;

            if (containingType != null)
            {
                foreach (INamedTypeSymbol interfaceSymbol in containingType.Interfaces)
                {
                    foreach (ISymbol memberSymbol in interfaceSymbol.GetMembers(symbol.Name))
                    {
                        if (SymbolEqualityComparer.Default.Equals(symbol, containingType.FindImplementationForInterfaceMember(memberSymbol)))
                        {
                            XElement element = getDocumentation(memberSymbol);

                            return (ContainsInheritDoc(element))
                                ? FindInheritedDocumentationFromImplementedInterfaceMember(memberSymbol, getDocumentation)
                                : element;
                        }
                    }
                }
            }

            return null;
        }

        public static XElement FindInheritedDocumentationFromBaseType(INamedTypeSymbol namedTypeSymbol, Func<ISymbol, XElement> getDocumentation)
        {
            foreach (INamedTypeSymbol baseType in namedTypeSymbol.BaseTypes())
            {
                XElement element = getDocumentation(baseType);

                if (!ContainsInheritDoc(element))
                    return element;
            }

            return null;
        }

        public static XElement FindInheritedDocumentationFromImplementedInterface(INamedTypeSymbol namedTypeSymbol, Func<ISymbol, XElement> getDocumentation)
        {
            foreach (INamedTypeSymbol interfaceSymbol in namedTypeSymbol.Interfaces)
            {
                XElement element = getDocumentation(interfaceSymbol);

                return (ContainsInheritDoc(element))
                    ? FindInheritedDocumentationFromImplementedInterface(interfaceSymbol, getDocumentation)
                    : element;
            }

            return null;
        }

        public static bool ContainsInheritDoc(XElement element)
        {
            return element is not null
                && element.Elements().Count() == 1
                && string.Equals(element.Elements().First().Name.LocalName, "inheritdoc", StringComparison.OrdinalIgnoreCase);
        }
    }
}
