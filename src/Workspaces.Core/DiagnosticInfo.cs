﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;

namespace Roslynator;

internal readonly struct DiagnosticInfo
{
    public DiagnosticInfo(DiagnosticDescriptor descriptor, FileLinePositionSpan lineSpan, DiagnosticSeverity severity)
    {
        Descriptor = descriptor;
        LineSpan = lineSpan;
        Severity = severity;
    }

    public DiagnosticDescriptor Descriptor { get; }

    public FileLinePositionSpan LineSpan { get; }

    public DiagnosticSeverity Severity { get; }

    public static DiagnosticInfo Create(Diagnostic diagnostic)
    {
        return new(diagnostic.Descriptor, diagnostic.Location.GetMappedLineSpan(), diagnostic.Severity);
    }
}
