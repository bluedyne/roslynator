﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.Build.Locator;

namespace Roslynator.CommandLine;

public sealed class VisualStudioInstanceComparer : EqualityComparer<VisualStudioInstance>
{
    public static VisualStudioInstanceComparer MSBuildPath { get; } = new();

    private VisualStudioInstanceComparer()
    {
    }

    public override bool Equals(VisualStudioInstance x, VisualStudioInstance y)
    {
        if (object.ReferenceEquals(x, y))
            return true;

        if (x is null)
            return false;

        if (y is null)
            return false;

        return FileSystemHelpers.Comparer.Equals(x.MSBuildPath, y.MSBuildPath);
    }

    public override int GetHashCode(VisualStudioInstance obj)
    {
        return (obj is null) ? 0 : FileSystemHelpers.Comparer.GetHashCode(obj.MSBuildPath);
    }
}
