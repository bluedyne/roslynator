﻿// Copyright (c) Josef Pihrt and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Runtime.InteropServices;
using Xunit;
using static Roslynator.GeneratedCodeUtility;

namespace Roslynator.Testing.CSharp;

public static class GeneratedCodeUtilityTests
{
    [Fact]
    public static void TestIsGeneratedCodeFile()
    {
        string basePath = (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            ? "c:/foo/"
            : "/foo/";

        Assert.True(IsGeneratedCodeFile("TemporaryGeneratedFile_"));
        Assert.True(IsGeneratedCodeFile("TemporaryGeneratedFile_Foo"));
        Assert.True(IsGeneratedCodeFile("TemporaryGeneratedFile_.cs"));
        Assert.True(IsGeneratedCodeFile("TemporaryGeneratedFile_Foo.cs"));

        Assert.True(IsGeneratedCodeFile($"{basePath}TemporaryGeneratedFile_.cs"));
        Assert.True(IsGeneratedCodeFile($"{basePath}TemporaryGeneratedFile_Foo.cs"));

        Assert.True(IsGeneratedCodeFile(".designer.cs"));
        Assert.True(IsGeneratedCodeFile(".generated.cs"));
        Assert.True(IsGeneratedCodeFile(".g.cs"));
        Assert.True(IsGeneratedCodeFile(".g.i.cs"));
        Assert.True(IsGeneratedCodeFile(".AssemblyAttributes.cs"));

        Assert.True(IsGeneratedCodeFile("Foo.designer.cs"));
        Assert.True(IsGeneratedCodeFile("Foo.generated.cs"));
        Assert.True(IsGeneratedCodeFile("Foo.g.cs"));
        Assert.True(IsGeneratedCodeFile("Foo.g.i.cs"));
        Assert.True(IsGeneratedCodeFile("Foo.AssemblyAttributes.cs"));

        Assert.True(IsGeneratedCodeFile($"{basePath}.designer.cs"));
        Assert.True(IsGeneratedCodeFile($"{basePath}.generated.cs"));
        Assert.True(IsGeneratedCodeFile($"{basePath}.g.cs"));
        Assert.True(IsGeneratedCodeFile($"{basePath}.g.i.cs"));
        Assert.True(IsGeneratedCodeFile($"{basePath}.AssemblyAttributes.cs"));

        Assert.True(IsGeneratedCodeFile($"{basePath}Foo.designer.cs"));
        Assert.True(IsGeneratedCodeFile($"{basePath}Foo.generated.cs"));
        Assert.True(IsGeneratedCodeFile($"{basePath}Foo.g.cs"));
        Assert.True(IsGeneratedCodeFile($"{basePath}Foo.g.i.cs"));
        Assert.True(IsGeneratedCodeFile($"{basePath}Foo.AssemblyAttributes.cs"));

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Assert.True(IsGeneratedCodeFile("c:TemporaryGeneratedFile_.cs"));
            Assert.True(IsGeneratedCodeFile("c:TemporaryGeneratedFile_Foo.cs"));

            Assert.True(IsGeneratedCodeFile("c:.designer.cs"));
            Assert.True(IsGeneratedCodeFile("c:.generated.cs"));
            Assert.True(IsGeneratedCodeFile("c:.g.cs"));
            Assert.True(IsGeneratedCodeFile("c:.g.i.cs"));
            Assert.True(IsGeneratedCodeFile("c:.AssemblyAttributes.cs"));

            Assert.True(IsGeneratedCodeFile("c:Foo.designer.cs"));
            Assert.True(IsGeneratedCodeFile("c:Foo.generated.cs"));
            Assert.True(IsGeneratedCodeFile("c:Foo.g.cs"));
            Assert.True(IsGeneratedCodeFile("c:Foo.g.i.cs"));
            Assert.True(IsGeneratedCodeFile("c:Foo.AssemblyAttributes.cs"));
        }
    }

    [Fact]
    public static void TestIsNotGeneratedCodeFile()
    {
        Assert.False(IsGeneratedCodeFile(null));
        Assert.False(IsGeneratedCodeFile(""));
        Assert.False(IsGeneratedCodeFile(" "));
        Assert.False(IsGeneratedCodeFile("."));
        Assert.False(IsGeneratedCodeFile(@"\"));
        Assert.False(IsGeneratedCodeFile("foo"));
        Assert.False(IsGeneratedCodeFile("foo."));
        Assert.False(IsGeneratedCodeFile(@"foo\"));
        Assert.False(IsGeneratedCodeFile(@"foo\."));
        Assert.False(IsGeneratedCodeFile(@"c:\foo"));
        Assert.False(IsGeneratedCodeFile(@"c:\foo\"));
        Assert.False(IsGeneratedCodeFile("c:foo"));
        Assert.False(IsGeneratedCodeFile(@"c:foo\"));

        Assert.False(IsGeneratedCodeFile("Foo.designer"));
        Assert.False(IsGeneratedCodeFile("Foo.generated"));
        Assert.False(IsGeneratedCodeFile("Foo.g"));
        Assert.False(IsGeneratedCodeFile("Foo.AssemblyAttributes"));
    }
}
