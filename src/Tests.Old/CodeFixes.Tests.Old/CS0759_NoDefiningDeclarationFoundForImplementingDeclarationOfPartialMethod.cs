﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;

namespace Roslynator.CSharp.CodeFixes.Tests
{
    internal partial class CS0759_NoDefiningDeclarationFoundForImplementingDeclarationOfPartialMethod
    {
        private partial class Foo
        {
            partial object Bar()
            {
                return null;
            }
        }

    }
}
