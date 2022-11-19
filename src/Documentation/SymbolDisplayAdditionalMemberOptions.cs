﻿// Copyright (c) Josef Pihrt and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Roslynator.Documentation;

[Flags]
public enum SymbolDisplayAdditionalMemberOptions
{
    None = 0,
    UseItemPropertyName = 1,
    UseOperatorName = 1 << 1,
}
