﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace Roslynator.CSharp.Analyzers.Tests
{
    internal static class DeclareEachAttributeSeparately
    {
        [Obsolete, Conditional("DEBUG")]
        private static void Foo()
        {
        }
    }
}
