﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Threading;
using Microsoft.CodeAnalysis;

namespace Roslynator;

internal static class RoslynUtility
{
    private static ImmutableHashSet<string> _wellKnownLanguageNames;

    public static ImmutableHashSet<string> WellKnownLanguageNames
    {
        get
        {
            if (_wellKnownLanguageNames is null)
                Interlocked.CompareExchange(ref _wellKnownLanguageNames, LoadLanguageNames(), null);

            return _wellKnownLanguageNames;

            static ImmutableHashSet<string> LoadLanguageNames()
            {
                return typeof(LanguageNames)
                    .GetRuntimeFields()
                    .Where(f => f.IsPublic)
                    .Select(f => (string)f.GetValue(null))
                    .ToImmutableHashSet();
            }
        }
    }
}
