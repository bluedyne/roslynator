﻿// Copyright (c) Josef Pihrt and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Roslynator.CSharp.CodeFixes;
using Roslynator.Testing.CSharp;
using Xunit;

namespace Roslynator.CSharp.Analysis.Tests;

public class RCS1235OptimizeMethodCallTests : AbstractCSharpDiagnosticVerifier<InvocationExpressionAnalyzer, OptimizeMethodCallCodeFixProvider>
{
    public override DiagnosticDescriptor Descriptor { get; } = DiagnosticRules.OptimizeMethodCall;

    [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.OptimizeMethodCall)]
    public async Task Test_CallCompareOrdinalInsteadOfCompare()
    {
        await VerifyDiagnosticAndFixAsync(@"
using System;

class C
{
    void M()
    {
        string x = null;
        string y = null;

        var result = [|string.Compare(x, y, StringComparison.Ordinal)|];
    }
}
", @"
using System;

class C
{
    void M()
    {
        string x = null;
        string y = null;

        var result = string.CompareOrdinal(x, y);
    }
}
");
    }

    [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.OptimizeMethodCall)]
    public async Task Test_CallEqualsInsteadOfCompare()
    {
        await VerifyDiagnosticAndFixAsync(@"
using System;

class C
{
    void M()
    {
        string x = null;
        string y = null;

        if ([|string.Compare(x, y, StringComparison.Ordinal) == 0|])
        {
        }
    }
}
", @"
using System;

class C
{
    void M()
    {
        string x = null;
        string y = null;

        if (string.Equals(x, y, StringComparison.Ordinal))
        {
        }
    }
}
");
    }

    [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.OptimizeMethodCall)]
    public async Task Test_CallNotEqualsInsteadOfCompare()
    {
        await VerifyDiagnosticAndFixAsync(@"
using System;

class C
{
    void M()
    {
        string x = null;
        string y = null;

        if ([|string.Compare(x, y, StringComparison.Ordinal) != 0|])
        {
        }
    }
}
", @"
using System;

class C
{
    void M()
    {
        string x = null;
        string y = null;

        if (!string.Equals(x, y, StringComparison.Ordinal))
        {
        }
    }
}
");
    }

    [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.OptimizeMethodCall)]
    public async Task Test_CallStringConcatInsteadOfStringJoin_EmptyStringLiteral()
    {
        await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M()
    {
        string s = [|string.Join("""", default(object), default(object))|];
    }
}
", @"
class C
{
    void M()
    {
        string s = string.Concat(default(object), default(object));
    }
}
");
    }

    [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.OptimizeMethodCall)]
    public async Task Test_CallStringConcatInsteadOfStringJoin_EmptyStringLiteral2()
    {
        await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M()
    {
        string s = [|string.Join("""", ""a"", ""b"")|];
    }
}
", @"
class C
{
    void M()
    {
        string s = string.Concat(""a"", ""b"");
    }
}
");
    }

    [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.OptimizeMethodCall)]
    public async Task Test_CallStringConcatInsteadOfStringJoin_StringEmpty()
    {
        await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M()
    {
        string s = [|string.Join(string.Empty, new string[] { """" })|];
    }
}
", @"
class C
{
    void M()
    {
        string s = string.Concat(new string[] { """" });
    }
}
");
    }

    [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.OptimizeMethodCall)]
    public async Task Test_CallStringConcatInsteadOfStringJoin_EmptyStringConstant()
    {
        await VerifyDiagnosticAndFixAsync(@"
class C
{
    const string EmptyString = """";

    void M()
    {
        string s = [|string.Join(EmptyString, new object[] { """" })|];
    }
}
", @"
class C
{
    const string EmptyString = """";

    void M()
    {
        string s = string.Concat(new object[] { """" });
    }
}
");
    }

    [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.OptimizeMethodCall)]
    public async Task Test_CallDebugFailInsteadOfDebugAssert()
    {
        await VerifyDiagnosticAndFixAsync(@"
using System.Diagnostics;

class C
{
    void M()
    {
        [|Debug.Assert(false)|];
    }
}
", @"
using System.Diagnostics;

class C
{
    void M()
    {
        Debug.Fail("""");
    }
}
");
    }

    [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.OptimizeMethodCall)]
    public async Task Test_CallDebugFailInsteadOfDebugAssert2()
    {
        await VerifyDiagnosticAndFixAsync(@"
using System.Diagnostics;

class C
{
    void M()
    {
        [|Debug.Assert(false, ""x"")|];
    }
}
", @"
using System.Diagnostics;

class C
{
    void M()
    {
        Debug.Fail(""x"");
    }
}
");
    }

    [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.OptimizeMethodCall)]
    public async Task Test_CallDebugFailInsteadOfDebugAssert3()
    {
        await VerifyDiagnosticAndFixAsync(@"
using System.Diagnostics;

class C
{
    void M()
    {
        [|Debug.Assert(false, ""x"", ""y"")|];
    }
}
", @"
using System.Diagnostics;

class C
{
    void M()
    {
        Debug.Fail(""x"", ""y"");
    }
}
");
    }

    [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.OptimizeMethodCall)]
    public async Task Test_OptimizeDictionaryContainsKey()
    {
        await VerifyDiagnosticAndFixAsync(@"
using System.Collections.Generic;

class C
{
    void M()
    {
        var dic = new Dictionary<string, string>();
        string key = null;
        string value = null;

        [|if (dic.ContainsKey(key))
        {
            dic[key] = value;
        }
        else
        {
            dic.Add(key, value);
        }|]
    }
}
", @"
using System.Collections.Generic;

class C
{
    void M()
    {
        var dic = new Dictionary<string, string>();
        string key = null;
        string value = null;

        dic[key] = value;
    }
}
");
    }

    [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.OptimizeMethodCall)]
    public async Task Test_OptimizeDictionaryContainsKey_EmbeddedStatement()
    {
        await VerifyDiagnosticAndFixAsync(@"
using System.Collections.Generic;

class C
{
    void M()
    {
        var dic = new Dictionary<string, string>();
        string key = null;
        string value = null;

        [|if (dic.ContainsKey(key))
            dic[key] = value;
        else
            dic.Add(key, value);|]
    }
}
", @"
using System.Collections.Generic;

class C
{
    void M()
    {
        var dic = new Dictionary<string, string>();
        string key = null;
        string value = null;

        dic[key] = value;
    }
}
");
    }

    [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.OptimizeMethodCall)]
    public async Task Test_OptimizeDictionaryContainsKey_LogicalNot()
    {
        await VerifyDiagnosticAndFixAsync(@"
using System.Collections.Generic;

class C
{
    void M()
    {
        var dic = new Dictionary<string, string>();
        string key = null;
        string value = null;

        [|if (!dic.ContainsKey(key))
        {
            dic.Add(key, value);
        }
        else
        {
            dic[key] = value;
        }|]
    }
}
", @"
using System.Collections.Generic;

class C
{
    void M()
    {
        var dic = new Dictionary<string, string>();
        string key = null;
        string value = null;

        dic[key] = value;
    }
}
");
    }

    [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.OptimizeMethodCall)]
    public async Task Test_OptimizeAdd()
    {
        await VerifyDiagnosticAndFixAsync(@"
using System.Collections.Generic;

class C
{
    void M()
    {
        var items = new List<string>();

        foreach (var item in items)
        {
            items.[|Add|](item);
        }
    }
}
", @"
using System.Collections.Generic;

class C
{
    void M()
    {
        var items = new List<string>();

        items.AddRange(items);
    }
}
");
    }

    [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.OptimizeMethodCall)]
    public async Task Test_OptimizeAdd_EmbeddedStatement()
    {
        await VerifyDiagnosticAndFixAsync(@"
using System.Collections.Generic;

class C
{
    void M()
    {
        var items = new List<string>();

        foreach (var item in items)
            items.[|Add|](item);
    }
}
", @"
using System.Collections.Generic;

class C
{
    void M()
    {
        var items = new List<string>();

        items.AddRange(items);
    }
}
");
    }

    [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.OptimizeMethodCall)]
    public async Task Test_NoDiagnostic_OptimizeAdd()
    {
        await VerifyNoDiagnosticAsync(@"
using System.Collections.Generic;

class C
{
    void M()
    {
        var items = new List<string>();

        foreach (var item in items)
        {
            M();
            items.Add(item);
        }
    }
}
");
    }

    [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.OptimizeMethodCall)]
    public async Task TestNoDiagnostic_OptimizeAdd_Await()
    {
        await VerifyNoDiagnosticAsync(@"
using System.Threading.Tasks;
using System.Collections.Generic;

class C
{
    async Task M()
    {
        var items = new List<string>();
        IAsyncEnumerable<string> items2 = null;

        await foreach (var item in items2)
            items.Add(item);
    }
}
");
    }

    [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.OptimizeMethodCall)]
    public async Task TestNoDiagnostic_OptimizeAdd_Await2()
    {
        await VerifyNoDiagnosticAsync(@"
using System.Threading.Tasks;
using System.Collections.Generic;

class C
{
    async Task M()
    {
        var items = new List<string>();
        IAsyncEnumerable<string> items2 = null;

        await foreach (var item in items2)
        {
            items.Add(item);
        }

    }
}
");
    }

    [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.OptimizeMethodCall)]
    public async Task TestNoDiagnostic_OptimizeAdd_DifferentType()
    {
        await VerifyNoDiagnosticAsync(@"
using System.Collections.Generic;
using System.Collections.ObjectModel;

class C
{
    public void M()
    {
        var collection = new ObjectCollection();
        var dictionary = new Dictionary<string, object>();

        foreach (object item in dictionary.Values)
        {
            collection.Add(item);
        }
    }

    class ObjectCollection : Collection<object>
    {
        public void AddRange(ObjectCollection other)
        {
        }
    }
}
");
    }

    [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.OptimizeMethodCall)]
    public async Task TestNoDiagnostic_CallCompareOrdinalInsteadOfCompare_NotStringComparisonOrdinal()
    {
        await VerifyNoDiagnosticAsync(@"
using System;

class C
{
    void M()
    {
        string x = null;
        string y = null;

        var result = string.Compare(x, y, StringComparison.CurrentCulture);
    }
}
");
    }

    [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.OptimizeMethodCall)]
    public async Task TestNoDiagnostic_CallStringConcatInsteadOfStringJoin()
    {
        await VerifyNoDiagnosticAsync(@"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

class C
{
    void M()
    {
        string s = string.Join(""x"", new object[] { """" });
    }
}
");
    }

    [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.OptimizeMethodCall)]
    public async Task TestNoDiagnostic_ForEachVariableUsedInExpression()
    {
        await VerifyNoDiagnosticAsync(@"
using System.Collections.Generic;

class C
{
    (List<int>, List<int>) M(List<int> list)
    {
        var positive = new List<int>();
        var negative = new List<int>();

        foreach (int i in list)
        {
            ((i >= 0) ? positive : negative).Add(i);
        }

        return (positive, negative);
    }
}
");
    }

    [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.OptimizeMethodCall)]
    public async Task TestNoDiagnostic_Recursion()
    {
        await VerifyNoDiagnosticAsync(@"
using System.Collections.Generic;
using System.Collections.ObjectModel;

class MyCollection : Collection<string>
{
    public void AddRange(IEnumerable<string> data)
    {
        foreach (string item in data)
        {
            this.Add(item);
        }
    }
}
");
    }
}
