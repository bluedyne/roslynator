﻿// Copyright (c) Josef Pihrt and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Roslynator.CommandLine;

internal enum CommandStatus
{
    Success = 0,
    NotSuccess = 1,
    Fail = 2,
    Canceled = 3,
}
