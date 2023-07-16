﻿// Copyright (c) Josef Pihrt and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslynator;

internal static class DiagnosticHelpers
{
    #region SymbolAnalysisContext
    public static void ReportDiagnostic(
        SymbolAnalysisContext context,
        DiagnosticDescriptor descriptor,
        SyntaxNode node,
        params object[] messageArgs)
    {
        ReportDiagnostic(
            context: context,
            descriptor: descriptor,
            location: node.GetLocation(),
            messageArgs: messageArgs);
    }

    public static void ReportDiagnostic(
        SymbolAnalysisContext context,
        DiagnosticDescriptor descriptor,
        SyntaxToken token,
        params object[] messageArgs)
    {
        ReportDiagnostic(
            context: context,
            descriptor: descriptor,
            location: token.GetLocation(),
            messageArgs: messageArgs);
    }

    public static void ReportDiagnostic(
        SymbolAnalysisContext context,
        DiagnosticDescriptor descriptor,
        SyntaxTrivia trivia,
        params object[] messageArgs)
    {
        ReportDiagnostic(
            context: context,
            descriptor: descriptor,
            location: trivia.GetLocation(),
            messageArgs: messageArgs);
    }

    public static void ReportDiagnostic(
        SymbolAnalysisContext context,
        DiagnosticDescriptor descriptor,
        Location location,
        params object[] messageArgs)
    {
        ReportDiagnostic(
            context,
            CreateDiagnostic(descriptor, location, messageArgs));
    }

    public static void ReportDiagnostic(
        SymbolAnalysisContext context,
        DiagnosticDescriptor descriptor,
        Location location,
        IEnumerable<Location> additionalLocations,
        params object[] messageArgs)
    {
        ReportDiagnostic(
            context,
            CreateDiagnostic(descriptor, location, additionalLocations, messageArgs));
    }

    public static void ReportDiagnostic(
        SymbolAnalysisContext context,
        DiagnosticDescriptor descriptor,
        Location location,
        ImmutableDictionary<string, string> properties,
        params object[] messageArgs)
    {
        ReportDiagnostic(
            context,
            CreateDiagnostic(descriptor, location, properties, messageArgs));
    }

    public static void ReportDiagnostic(
        SymbolAnalysisContext context,
        DiagnosticDescriptor descriptor,
        Location location,
        IEnumerable<Location> additionalLocations,
        ImmutableDictionary<string, string> properties,
        params object[] messageArgs)
    {
        ReportDiagnostic(
            context,
            CreateDiagnostic(descriptor, location, additionalLocations, properties, messageArgs));
    }

    private static void ReportDiagnostic(SymbolAnalysisContext context, Diagnostic diagnostic)
    {
        VerifyDiagnostic(diagnostic);

        context.ReportDiagnostic(diagnostic);
    }
    #endregion SymbolAnalysisContext

    #region SyntaxNodeAnalysisContext
    public static void ReportDiagnosticIfEffective(
        SyntaxNodeAnalysisContext context,
        DiagnosticDescriptor descriptor,
        SyntaxNode node,
        params object[] messageArgs)
    {
        if (descriptor.IsEffective(context))
            ReportDiagnostic(context, descriptor, node, messageArgs);
    }

    public static void ReportDiagnosticIfEffective(
        SyntaxNodeAnalysisContext context,
        DiagnosticDescriptor descriptor,
        Location location,
        params object[] messageArgs)
    {
        if (descriptor.IsEffective(context))
            ReportDiagnostic(context, descriptor, location, messageArgs);
    }

    public static void ReportDiagnostic(
        SyntaxNodeAnalysisContext context,
        DiagnosticDescriptor descriptor,
        SyntaxNode node,
        params object[] messageArgs)
    {
        ReportDiagnostic(
            context: context,
            descriptor: descriptor,
            location: node.GetLocation(),
            messageArgs: messageArgs);
    }

    public static void ReportDiagnostic(
        SyntaxNodeAnalysisContext context,
        DiagnosticDescriptor descriptor,
        SyntaxToken token,
        params object[] messageArgs)
    {
        ReportDiagnostic(
            context: context,
            descriptor: descriptor,
            location: token.GetLocation(),
            messageArgs: messageArgs);
    }

    public static void ReportDiagnostic(
        SyntaxNodeAnalysisContext context,
        DiagnosticDescriptor descriptor,
        SyntaxTrivia trivia,
        params object[] messageArgs)
    {
        ReportDiagnostic(
            context: context,
            descriptor: descriptor,
            location: trivia.GetLocation(),
            messageArgs: messageArgs);
    }

    public static void ReportDiagnostic(
        SyntaxNodeAnalysisContext context,
        DiagnosticDescriptor descriptor,
        Location location,
        params object[] messageArgs)
    {
        ReportDiagnostic(
            context,
            CreateDiagnostic(descriptor, location, messageArgs));
    }

    public static void ReportDiagnostic(
        SyntaxNodeAnalysisContext context,
        DiagnosticDescriptor descriptor,
        Location location,
        IEnumerable<Location> additionalLocations,
        params object[] messageArgs)
    {
        ReportDiagnostic(
            context,
            CreateDiagnostic(descriptor, location, additionalLocations, messageArgs));
    }

    public static void ReportDiagnostic(
        SyntaxNodeAnalysisContext context,
        DiagnosticDescriptor descriptor,
        Location location,
        ImmutableDictionary<string, string> properties,
        params object[] messageArgs)
    {
        ReportDiagnostic(
            context,
            CreateDiagnostic(descriptor, location, properties, messageArgs));
    }

    public static void ReportDiagnostic(
        SyntaxNodeAnalysisContext context,
        DiagnosticDescriptor descriptor,
        Location location,
        IEnumerable<Location> additionalLocations,
        ImmutableDictionary<string, string> properties,
        params object[] messageArgs)
    {
        ReportDiagnostic(
            context,
            CreateDiagnostic(descriptor, location, additionalLocations, properties, messageArgs));
    }

    public static void ReportToken(SyntaxNodeAnalysisContext context, DiagnosticDescriptor descriptor, SyntaxToken token, params object[] messageArgs)
    {
        if (!token.IsMissing)
            ReportDiagnostic(context, descriptor, token, messageArgs);
    }

    public static void ReportNode(SyntaxNodeAnalysisContext context, DiagnosticDescriptor descriptor, SyntaxNode node, params object[] messageArgs)
    {
        if (!node.IsMissing)
            ReportDiagnostic(context, descriptor, node, messageArgs);
    }

    public static void ReportDiagnostic(SyntaxNodeAnalysisContext context, Diagnostic diagnostic)
    {
        VerifyDiagnostic(diagnostic);

        context.ReportDiagnostic(diagnostic);
    }
    #endregion SyntaxNodeAnalysisContext

    #region SyntaxTreeAnalysisContext
    public static void ReportDiagnostic(
        SyntaxTreeAnalysisContext context,
        DiagnosticDescriptor descriptor,
        SyntaxNode node,
        params object[] messageArgs)
    {
        ReportDiagnostic(
            context: context,
            descriptor: descriptor,
            location: node.GetLocation(),
            messageArgs: messageArgs);
    }

    public static void ReportDiagnostic(
        SyntaxTreeAnalysisContext context,
        DiagnosticDescriptor descriptor,
        SyntaxToken token,
        params object[] messageArgs)
    {
        ReportDiagnostic(
            context: context,
            descriptor: descriptor,
            location: token.GetLocation(),
            messageArgs: messageArgs);
    }

    public static void ReportDiagnostic(
        SyntaxTreeAnalysisContext context,
        DiagnosticDescriptor descriptor,
        SyntaxTrivia trivia,
        params object[] messageArgs)
    {
        ReportDiagnostic(
            context: context,
            descriptor: descriptor,
            location: trivia.GetLocation(),
            messageArgs: messageArgs);
    }

    public static void ReportDiagnostic(
        SyntaxTreeAnalysisContext context,
        DiagnosticDescriptor descriptor,
        Location location,
        params object[] messageArgs)
    {
        ReportDiagnostic(
            context,
            CreateDiagnostic(descriptor, location, messageArgs));
    }

    public static void ReportDiagnostic(
        SyntaxTreeAnalysisContext context,
        DiagnosticDescriptor descriptor,
        Location location,
        IEnumerable<Location> additionalLocations,
        params object[] messageArgs)
    {
        ReportDiagnostic(
            context,
            CreateDiagnostic(descriptor, location, additionalLocations, messageArgs));
    }

    public static void ReportDiagnostic(
        SyntaxTreeAnalysisContext context,
        DiagnosticDescriptor descriptor,
        Location location,
        ImmutableDictionary<string, string> properties,
        params object[] messageArgs)
    {
        ReportDiagnostic(
            context,
            CreateDiagnostic(descriptor, location, properties, messageArgs));
    }

    public static void ReportDiagnostic(
        SyntaxTreeAnalysisContext context,
        DiagnosticDescriptor descriptor,
        Location location,
        IEnumerable<Location> additionalLocations,
        ImmutableDictionary<string, string> properties,
        params object[] messageArgs)
    {
        ReportDiagnostic(
            context,
            CreateDiagnostic(descriptor, location, additionalLocations, properties, messageArgs));
    }

    private static void ReportDiagnostic(SyntaxTreeAnalysisContext context, Diagnostic diagnostic)
    {
        VerifyDiagnostic(diagnostic);

        context.ReportDiagnostic(diagnostic);
    }
    #endregion SyntaxTreeAnalysisContext

    [Conditional("DEBUG")]
    private static void VerifyDiagnostic(Diagnostic diagnostic)
    {
        Debug.Assert(Regex.IsMatch(diagnostic.Id, @"\AR(C|O)S[0-9]{4}(FadeOut)?\z"), $"Invalid diagnostic id '{diagnostic.Id}'.");
    }

    internal static bool IsAnyEffective(SyntaxNodeAnalysisContext context, ImmutableArray<DiagnosticDescriptor> descriptors)
    {
        return IsAnyEffective(context.Compilation, descriptors);
    }

    internal static bool IsAnyEffective(SyntaxNodeAnalysisContext context, DiagnosticDescriptor descriptor1, DiagnosticDescriptor descriptor2)
    {
        return IsAnyEffective(context.Compilation, descriptor1, descriptor2);
    }

    internal static bool IsAnyEffective(SyntaxNodeAnalysisContext context, DiagnosticDescriptor descriptor1, DiagnosticDescriptor descriptor2, DiagnosticDescriptor descriptor3)
    {
        return IsAnyEffective(context.Compilation, descriptor1, descriptor2, descriptor3);
    }

    internal static bool IsAnyEffective(Compilation compilation, ImmutableArray<DiagnosticDescriptor> descriptors)
    {
        foreach (DiagnosticDescriptor descriptor in descriptors)
        {
            if (descriptor.IsEffective(compilation))
                return true;
        }

        return false;
    }

    internal static bool IsAnyEffective(Compilation compilation, DiagnosticDescriptor descriptor1, DiagnosticDescriptor descriptor2)
    {
        return descriptor1.IsEffective(compilation)
            || descriptor2.IsEffective(compilation);
    }

    internal static bool IsAnyEffective(Compilation compilation, DiagnosticDescriptor descriptor1, DiagnosticDescriptor descriptor2, DiagnosticDescriptor descriptor3)
    {
        return descriptor1.IsEffective(compilation)
            || descriptor2.IsEffective(compilation)
            || descriptor3.IsEffective(compilation);
    }

    private static Diagnostic CreateDiagnostic(DiagnosticDescriptor descriptor, Location location, object[] messageArgs)
    {
        VerifyMessageArgs(descriptor, messageArgs);

        return Diagnostic.Create(
            descriptor: descriptor,
            location: location,
            messageArgs: messageArgs);
    }

    private static Diagnostic CreateDiagnostic(DiagnosticDescriptor descriptor, Location location, IEnumerable<Location> additionalLocations, object[] messageArgs)
    {
        VerifyMessageArgs(descriptor, messageArgs);

        return Diagnostic.Create(
            descriptor: descriptor,
            location: location,
            additionalLocations: additionalLocations,
            messageArgs: messageArgs);
    }

    private static Diagnostic CreateDiagnostic(DiagnosticDescriptor descriptor, Location location, ImmutableDictionary<string, string> properties, object[] messageArgs)
    {
        VerifyMessageArgs(descriptor, messageArgs);

        return Diagnostic.Create(
            descriptor: descriptor,
            location: location,
            properties: properties,
            messageArgs: messageArgs);
    }

    private static Diagnostic CreateDiagnostic(DiagnosticDescriptor descriptor, Location location, IEnumerable<Location> additionalLocations, ImmutableDictionary<string, string> properties, object[] messageArgs)
    {
        VerifyMessageArgs(descriptor, messageArgs);

        return Diagnostic.Create(
            descriptor: descriptor,
            location: location,
            additionalLocations: additionalLocations,
            properties: properties,
            messageArgs: messageArgs);
    }

    [Conditional("DEBUG")]
    private static void VerifyMessageArgs(DiagnosticDescriptor descriptor, object[] messageArgs)
    {
        string message = descriptor.MessageFormat.ToString();
        int count = Regex.Matches(message, @"\{\d\}").Count;

        if (count != messageArgs.Length)
        {
            throw new InvalidOperationException("Invalid number of message arguments. "
                + $"Message: {message}, "
                + $"Arguments: {string.Join(", ", messageArgs)}");
        }
    }
}
