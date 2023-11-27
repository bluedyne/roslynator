﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.ObjectModel;

namespace Roslynator.CSharp.Analyzers.Tests
{
    public static class ReorderModifiers
    {
        internal protected class Foo
        {
        }

        public public class Foo2
        {
        }
    }
}