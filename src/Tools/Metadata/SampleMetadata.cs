﻿// Copyright (c) Josef Pihrt and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Roslynator.Metadata;

public record struct SampleMetadata(string Before, string After)
{
    public SampleMetadata WithBefore(string before)
    {
        return new SampleMetadata(Before: before, After: After);
    }
}
