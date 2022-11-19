﻿// Copyright (c) Josef Pihrt and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Roslynator.CommandLine;

[Flags]
internal enum MetadataReferenceFilter
{
    None = 0,
    Dll = 1,
    Project = 1 << 1,
}
