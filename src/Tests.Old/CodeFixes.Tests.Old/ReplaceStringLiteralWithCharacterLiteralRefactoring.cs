﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Roslynator.CSharp.CodeFixes.Tests
{
    internal class ReplaceStringLiteralWithCharacterLiteralRefactoring
    {
        public char SomeMethod()
        {
            string s = "x";

            char ch = @"""";

            ch = "'";
            ch = @"'";

            ch = "\"";
            ch = @"""";

            ch = "\\";
            ch = @"\";

            ch = "	";
            ch = "\t";
            ch = @"	";

            return ch;
        }
    }
}
