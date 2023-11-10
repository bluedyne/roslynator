﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Roslynator.Testing;

internal interface IAssert
{
    /// <summary>
    /// Compares specified values and throws error if they are not equal.
    /// </summary>
    /// <param name="expected"></param>
    /// <param name="actual"></param>
    void Equal(string expected, string actual);

    /// <summary>
    /// Throws an error if a condition is not equal to <c>true</c>.
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="userMessage"></param>
    void True(bool condition, string userMessage);

    /// <summary>
    /// Throws an error if <paramref name="value"/> is not <c>null</c>.
    /// </summary>
    void Null(object? value);

    /// <summary>
    /// Throws an error if <paramref name="value"/> is <c>null</c>.
    /// </summary>
    void NotNull(object? value);
}
