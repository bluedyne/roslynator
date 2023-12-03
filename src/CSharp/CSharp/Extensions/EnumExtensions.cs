﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.CSharp;

namespace Roslynator.CSharp;

/// <summary>
/// A set of extension methods for enumerations.
/// </summary>
internal static class EnumExtensions
{
    /// <summary>
    /// Returns true if the kind is one of the specified kinds.
    /// </summary>
    public static bool Is(this SyntaxKind kind, SyntaxKind kind1, SyntaxKind kind2)
    {
        return kind == kind1
            || kind == kind2;
    }

    /// <summary>
    /// Returns true if the kind is one of the specified kinds.
    /// </summary>
    public static bool Is(this SyntaxKind kind, SyntaxKind kind1, SyntaxKind kind2, SyntaxKind kind3)
    {
        return kind == kind1
            || kind == kind2
            || kind == kind3;
    }

    /// <summary>
    /// Returns true if the kind is one of the specified kinds.
    /// </summary>
    public static bool Is(this SyntaxKind kind, SyntaxKind kind1, SyntaxKind kind2, SyntaxKind kind3, SyntaxKind kind4)
    {
        return kind == kind1
            || kind == kind2
            || kind == kind3
            || kind == kind4;
    }

    /// <summary>
    /// Returns true if the kind is one of the specified kinds.
    /// </summary>
    public static bool Is(this SyntaxKind kind, SyntaxKind kind1, SyntaxKind kind2, SyntaxKind kind3, SyntaxKind kind4, SyntaxKind kind5)
    {
        return kind == kind1
            || kind == kind2
            || kind == kind3
            || kind == kind4
            || kind == kind5;
    }

    public static bool HasAnyFlag(this ModifierFilter modifierFilter, ModifierFilter value)
    {
        return (modifierFilter & value) != 0;
    }
}
