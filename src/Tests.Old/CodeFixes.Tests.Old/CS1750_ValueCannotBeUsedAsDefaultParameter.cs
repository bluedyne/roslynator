﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;

namespace Roslynator.CSharp.CodeFixes.Tests
{
    internal static class CS1750_ValueCannotBeUsedAsDefaultParameter
    {
        private static void Foo(CancellationToken cancellationToken = null)
        {
        }
    }
}
