﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;

namespace Roslynator.CSharp.Refactorings.Tests
{
    internal class ReplaceAnonymousMethodWithLambdaExpressionRefactoring
    {
        public void Do()
        {
            var items = new List<int>();


            var q = items.Select(delegate (int value)
            {
                return value + 1;
            });








        }
    }
}
