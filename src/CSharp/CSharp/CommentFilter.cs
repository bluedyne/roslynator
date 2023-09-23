﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Roslynator.CSharp;

/// <summary>
/// Specifies C# comments.
/// </summary>
[Flags]
public enum CommentFilter
{
    /// <summary>
    /// None comment specified.
    /// </summary>
    None = 0,

    /// <summary>
    /// Single-line comment.
    /// </summary>
    SingleLine = 1,

    /// <summary>
    /// Multi-line comment.
    /// </summary>
    MultiLine = 1 << 1,

    /// <summary>
    /// Non-documentation comment (single-line or multi-line).
    /// </summary>
    NonDocumentation = SingleLine | MultiLine,

    /// <summary>
    /// Single-line documentation comment.
    /// </summary>
    SingleLineDocumentation = 1 << 2,

    /// <summary>
    /// Multi-line documentation comment.
    /// </summary>
    MultiLineDocumentation = 1 << 3,

    /// <summary>
    /// Documentation comment (single-line or multi-line).
    /// </summary>
    Documentation = SingleLineDocumentation | MultiLineDocumentation,

    /// <summary>
    /// Documentation or non-documentation comment.
    /// </summary>
    All = NonDocumentation | Documentation,
}
