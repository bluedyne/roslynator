﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Roslynator.CSharp.CodeFixes.Tests
{
    internal static class ChangeTypeOfParamsParameter
    {
        private static void Foo(params object values)
        {
        }

        private static void Foo(params System.Action values)
        {
        }
    }
}
