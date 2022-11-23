﻿// Copyright (c) Josef Pihrt and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp;

internal sealed class EnumMemberDeclarationNameComparer : IComparer<EnumMemberDeclarationSyntax>
{
    private EnumMemberDeclarationNameComparer()
    {
    }

    public static readonly EnumMemberDeclarationNameComparer Instance = new();

    public int Compare(EnumMemberDeclarationSyntax x, EnumMemberDeclarationSyntax y)
    {
        return CompareCore(x, y);
    }

    private static int CompareCore(EnumMemberDeclarationSyntax x, EnumMemberDeclarationSyntax y)
    {
        if (object.ReferenceEquals(x, y))
            return 0;

        if (x is null)
            return -1;

        if (y is null)
            return 1;

        return string.Compare(x.Identifier.ValueText, y.Identifier.ValueText, StringComparison.CurrentCulture);
    }
}
