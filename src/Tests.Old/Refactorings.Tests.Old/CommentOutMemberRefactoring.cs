﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Roslynator.CSharp.Refactorings.Tests
{
    internal class CommentOutMemberRefactoring
    {
        public object GetValue()
        {
            return null;
        }

        public object GetValue2()
        {

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object GetValue3()
        {

            return null;
        }
    }
}
