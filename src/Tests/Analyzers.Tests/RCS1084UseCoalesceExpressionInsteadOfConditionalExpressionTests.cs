﻿// Copyright (c) Josef Pihrt and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Roslynator.CSharp.CodeFixes;
using Roslynator.Testing.CSharp;
using Xunit;

namespace Roslynator.CSharp.Analysis.Tests;

public class RCS1084UseCoalesceExpressionInsteadOfConditionalExpressionTests : AbstractCSharpDiagnosticVerifier<SimplifyNullCheckAnalyzer, ConditionalExpressionCodeFixProvider>
{
    public override DiagnosticDescriptor Descriptor { get; } = DiagnosticRules.UseCoalesceExpressionInsteadOfConditionalExpression;

    [Theory, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseCoalesceExpressionInsteadOfConditionalExpression)]
    [InlineData("s != null ? s : \"\"", "s ?? \"\"")]
    [InlineData("s == null ? \"\" : s", "s ?? \"\"")]
    [InlineData("(s != null) ? (s) : (\"\")", "s ?? \"\"")]
    [InlineData("(s == null) ? (\"\") : (s)", "s ?? \"\"")]
    public async Task Test_ReferenceType(string source, string expected)
    {
        await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M()
    {
        string s = null;

        s = [||];
    }
}
", source, expected);
    }

    [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseCoalesceExpressionInsteadOfConditionalExpression)]
    public async Task Test_PolymorphicType_WithNullable()
    {
        await VerifyDiagnosticAndFixAsync(@"
#nullable enable
class C
{
    private interface IBase { }

    private class A: IBase  { }
    
    private class B: IBase { }
    

    void M()
    {
        A? a = null;

        IBase c = [|a != null ? a : new B()|];
    }
}
", @"
#nullable enable
class C
{
    private interface IBase { }

    private class A: IBase  { }
    
    private class B: IBase { }
    

    void M()
    {
        A? a = null;

        IBase c = (IBase?)a ?? new B();
    }
}
");
    }

    [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseCoalesceExpressionInsteadOfConditionalExpression)]
    public async Task Test_PolymorphicType()
    {
        await VerifyDiagnosticAndFixAsync(@"
class C
{
    private interface IBase { }

    private class A: IBase  { }
    
    private class B: IBase { }
    

    void M()
    {
        A a = null;

        IBase c = [|a != null ? a : new B()|];
    }
}
", @"
class C
{
    private interface IBase { }

    private class A: IBase  { }
    
    private class B: IBase { }
    

    void M()
    {
        A a = null;

        IBase c = (IBase)a ?? new B();
    }
}
");
    }

    [Theory, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseCoalesceExpressionInsteadOfConditionalExpression)]
    [InlineData("(ni != null) ? ni.Value : 1", "ni ?? 1")]
    [InlineData("(ni == null) ? 1 : ni.Value", "ni ?? 1")]
    [InlineData("(ni.HasValue) ? ni.Value : 1", "ni ?? 1")]
    [InlineData("(!ni.HasValue) ? 1 : ni.Value", "ni ?? 1")]
    public async Task Test_ValueType(string source, string expected)
    {
        await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M()
    {
        int i = 0;
        int? ni = null;

        i = [||];
    }
}
", source, expected);
    }

    [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseCoalesceExpressionInsteadOfConditionalExpression)]
    public async Task TestNoDiagnostic()
    {
        await VerifyNoDiagnosticAsync(@"
class C
{
    public unsafe void M()
    {
        string s = """";

        s = (s != null) ? """" : s;
        s = (s == null) ? s : """";
    }
}
", options: Options.WithAllowUnsafe(true));
    }

    [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseCoalesceExpressionInsteadOfConditionalExpression)]
    public async Task TestNoDiagnostic_Pointer()
    {
        await VerifyNoDiagnosticAsync(@"
class C
{
    public unsafe void M()
    {
        int* i = null;

        i = (i == null) ? default(int*) : i;
        i = (i != null) ? i : default(int*);
    }
}
", options: Options.WithAllowUnsafe(true));
    }

    [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseCoalesceExpressionInsteadOfConditionalExpression)]
    public async Task TestNoDiagnostic_DefaultOfTEqualsToNull()
    {
        await VerifyNoDiagnosticAsync(@"
using System;

class C
{
    T M<T>()
    {
        return default(T) == null ? (T)Convert.ChangeType("""", typeof(T)) : default(T);
    }
}", options: Options.WithAllowUnsafe(true));
    }
}
