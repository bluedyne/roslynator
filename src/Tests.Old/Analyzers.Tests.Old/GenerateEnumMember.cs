﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Roslynator.CSharp.Analyzers.Tests
{
    public static class GenerateEnumMember
    {
        private enum Foo
        {
        }

        [Flags]
        private enum Foo2
        {
        }
    }
}
