﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Roslynator.CSharp.Refactorings.Tests
{
    internal static class ReplaceEqualsExpressionWithStringIsNullOrEmptyRefactoring
    {
        public static void Foo()
        {
            string s = null;

            if (s == null)
            {
            }

            if (s != null)
            {
            }
        }
    }
}
