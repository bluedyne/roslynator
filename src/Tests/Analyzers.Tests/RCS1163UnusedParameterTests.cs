﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Roslynator.CSharp.CodeFixes;
using Roslynator.Testing.CSharp;
using Xunit;

namespace Roslynator.CSharp.Analysis.Tests;

public class RCS1163UnusedParameterTests : AbstractCSharpDiagnosticVerifier<UnusedParameter.UnusedParameterAnalyzer, UnusedParameterCodeFixProvider>
{
    public override DiagnosticDescriptor Descriptor { get; } = DiagnosticRules.UnusedParameter;

    [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UnusedParameter)]
    public async Task Test_Method()
    {
        await VerifyDiagnosticAsync(@"
class C
{
    void M([|object p|], __arglist)
    {
    }
}
"
);
    }

    [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UnusedParameter)]
    public async Task Test_Lambda()
    {
        await VerifyDiagnosticAndFixAsync(@"
using System;

class C
{
    void M()
    {
        object _ = null;

        Action<string> action = [|f|] => M();
    }
}
", @"
using System;

class C
{
    void M()
    {
        object _ = null;

        Action<string> action = __ => M();
    }
}
");
    }

    [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UnusedParameter)]
    public async Task TestNoDiagnostic_StackAllocArrayCreationExpression()
    {
        await VerifyNoDiagnosticAsync(@"
class C
{
    unsafe void M(int length)
    {
        var memory = stackalloc byte[length];
    }
}
", options: Options.WithAllowUnsafe(true));
    }

    [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UnusedParameter)]
    public async Task TestNoDiagnostic_PartialMethod()
    {
        await VerifyNoDiagnosticAsync(@"
partial class C
{
    partial void M(object p);

    partial void M(object p)
    {
    }
}
");
    }

    [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UnusedParameter)]
    public async Task TestNoDiagnostic_MethodReferencedAsMethodGroup()
    {
        await VerifyNoDiagnosticAsync(@"
using System;

class C
{
    private void M(object p)
    {
        Action<object> action = M;
    }
}
");
    }

    [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UnusedParameter)]
    public async Task TestNoDiagnostic_ContainsOnlyThrowNew()
    {
        await VerifyNoDiagnosticAsync(@"
using System;

class C
{
    public C(object p)
    {
        throw new NotImplementedException();
    }

    public C(object p, object p2) => throw new NotImplementedException();

    public string this[int index] => throw new NotImplementedException();

    public string this[string index]
    {
        get { throw new NotImplementedException(); }
        set { throw new NotImplementedException(); }
    }

    public string this[string index, string index2]
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    public void Bar(object p) { throw new NotImplementedException(); }

    public object Bar(object p1, object p2) => throw new NotImplementedException();

    /// <summary>
    /// ...
    /// </summary>
    /// <param name=""p1""></param>
    /// <param name=""p2""></param>
    public void Bar2(object p1, object p2) { throw new NotImplementedException(); }

    public static C operator +(C left, C right)
    {
        throw new NotImplementedException();
    }

    public static explicit operator C(string value)
    {
        throw new NotImplementedException();
    }
}
");
    }

    [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UnusedParameter)]
    public async Task TestNoDiagnostic_SwitchExpression()
    {
        await VerifyNoDiagnosticAsync(@"
using System;

class C
{
    string M(StringSplitOptions options)
    {
        return options switch
        {
            _ => """"
        };
    }
}
");
    }

    [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UnusedParameter)]
    public async Task TestNoDiagnostic_Discard()
    {
        await VerifyNoDiagnosticAsync(@"
using System;

class C
{
    void M(string _, string __, string _1)
    {
    }
}
");
    }

    [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UnusedParameter)]
    public async Task TestNoDiagnostic_ArgIterator()
    {
        await VerifyNoDiagnosticAsync(@"
using System;

class C
{
    public static int GetCount(__arglist)
    {
        var argIterator = new ArgIterator(__arglist);
        return argIterator.GetRemainingCount();
    }
}
");
    }

    [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UnusedParameter)]
    public async Task TestNoDiagnostic_DependencyPropertyEventArgs()
    {
        await VerifyNoDiagnosticAsync(@"
using System;
using System.Windows;

static class C
{
    public static void M(this Foo foo)
    {
        foo.Changed += Foo_Changed;

        void Foo_Changed(object sender, DependencyPropertyChangedEventArgs e)
        {
        }
    }
}

public class Foo
{
    public event EventHandler<DependencyPropertyChangedEventArgs> Changed;
}

namespace System.Windows
{
    public class DependencyPropertyChangedEventArgs
    {
    }
}
 ");
    }

    [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UnusedParameter)]
    public async Task TestNoDiagnostic_StreamingContextAttributes()
    {
        await VerifyNoDiagnosticAsync(@"
using System;
using System.Runtime.Serialization;

class C
{
    [Obsolete]
    [OnSerialized]
    void M1(string p, StreamingContext context)
    {
        var x = p;
    }

    [OnDeserialized]
    void M2(StreamingContext context, string p)
    {
        var x = p;
    }

    [OnSerializing]
    void M3(StreamingContext context)
    {
    }

    [OnDeserializing]
    void M4(StreamingContext context)
    {
    }
}
 ");
    }
}
