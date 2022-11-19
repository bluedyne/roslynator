﻿// Copyright (c) Josef Pihrt and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Roslynator.Documentation;

[Flags]
internal enum WrapListOptions
{
    None = 0,
    Attributes = 1,
    Parameters = 1 << 1,
    BaseTypes = 1 << 2,
    Constraints = 1 << 3,
}
