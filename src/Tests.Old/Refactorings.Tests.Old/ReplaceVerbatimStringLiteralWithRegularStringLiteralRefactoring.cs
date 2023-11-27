﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Roslynator.CSharp.Refactorings.Tests
{
    internal class ReplaceVerbatimStringLiteralWithRegularStringLiteralRefactoring
    {
        public string SomeMethod()
        {

            string s = @"""1""
""2""";

            return s;
        }
    }
}
