﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using CommandLine;

namespace Roslynator.CommandLine;

[Verb("lloc", HelpText = "Counts logical lines of code in the specified project or solution.")]
public class LogicalLinesOfCodeCommandLineOptions : AbstractLinesOfCodeCommandLineOptions
{
}
