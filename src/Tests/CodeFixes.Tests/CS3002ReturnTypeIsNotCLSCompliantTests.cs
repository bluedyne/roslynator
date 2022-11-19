﻿// Copyright (c) Josef Pihrt and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Roslynator.Testing.CSharp;
using Xunit;

namespace Roslynator.CSharp.CodeFixes.Tests;

public class CS3002ReturnTypeIsNotCLSCompliantTests : AbstractCSharpCompilerDiagnosticFixVerifier<MemberDeclarationCodeFixProvider>
{
    public override string DiagnosticId { get; } = CompilerDiagnosticIdentifiers.CS3002_ReturnTypeIsNotCLSCompliant;

    [Fact, Trait(Traits.CodeFix, CompilerDiagnosticIdentifiers.CS3002_ReturnTypeIsNotCLSCompliant)]
    public async Task Test()
    {
        await VerifyFixAsync(@"
using System;

[assembly: CLSCompliant(true)]

public class C
{
    public ulong M()
    {
        return 0;
    }
}
", @"
using System;

[assembly: CLSCompliant(true)]

public class C
{
    [CLSCompliant(false)]
    public ulong M()
    {
        return 0;
    }
}
", equivalenceKey: EquivalenceKey.Create(DiagnosticId));
    }
}
