﻿// Copyright (c) Josef Pihrt and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.CodeAnalysis;

namespace Roslynator.CodeGeneration.CSharp;

internal static class RuntimeMetadataReference
{
    internal static readonly MetadataReference CorLibReference = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);

    private static readonly ImmutableDictionary<string, string> _assemblyMap = GetTrustedPlatformAssemblies();

    private static ImmutableDictionary<string, string> GetTrustedPlatformAssemblies()
    {
        return AppContext
            .GetData("TRUSTED_PLATFORM_ASSEMBLIES")
            .ToString()
            .Split(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? ';' : ':')
            .ToImmutableDictionary(f => Path.GetFileName(f));
    }

    public static PortableExecutableReference CreateFromAssemblyName(string assemblyName)
    {
        return MetadataReference.CreateFromFile(GetAssemblyLocation(assemblyName));
    }

    public static string GetAssemblyLocation(string assemblyName)
    {
        return _assemblyMap[assemblyName];
    }
}
