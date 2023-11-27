﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Roslynator.CSharp.Analyzers.Tests
{
    internal static class DefaultLabelShouldBeLastLabelInSwitchSection
    {
        private static void Foo()
        {
            string s = null;

            switch (s)
            {
                default:
                case "a":
                case "b":
                    break;
            }
        }
    }
}
