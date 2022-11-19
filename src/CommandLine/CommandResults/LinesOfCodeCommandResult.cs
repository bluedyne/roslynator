﻿// Copyright (c) Josef Pihrt and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Roslynator.CodeMetrics;

namespace Roslynator.CommandLine;

internal class LinesOfCodeCommandResult : CommandResult
{
    public LinesOfCodeCommandResult(CommandStatus status, CodeMetricsInfo metrics)
        : base(status)
    {
        Metrics = metrics;
    }

    public CodeMetricsInfo Metrics { get; }
}
