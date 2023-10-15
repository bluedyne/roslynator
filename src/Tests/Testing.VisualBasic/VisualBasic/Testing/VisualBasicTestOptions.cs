﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.VisualBasic;

namespace Roslynator.Testing;

public sealed class VisualBasicTestOptions : TestOptions
{
    public VisualBasicTestOptions(
        VisualBasicCompilationOptions? compilationOptions = null,
        VisualBasicParseOptions? parseOptions = null,
        IEnumerable<MetadataReference>? metadataReferences = null,
        IEnumerable<string>? allowedCompilerDiagnosticIds = null,
        DiagnosticSeverity allowedCompilerDiagnosticSeverity = DiagnosticSeverity.Info,
        IEnumerable<KeyValuePair<string, string>>? configOptions = null)
        : base(metadataReferences, allowedCompilerDiagnosticIds, allowedCompilerDiagnosticSeverity, configOptions)
    {
        CompilationOptions = compilationOptions ?? new VisualBasicCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
        ParseOptions = parseOptions ?? VisualBasicParseOptions.Default;
    }

    private VisualBasicTestOptions(VisualBasicTestOptions other)
        : base(
            other.MetadataReferences,
            other.AllowedCompilerDiagnosticIds,
            other.AllowedCompilerDiagnosticSeverity,
            other.ConfigOptions)
    {
        CompilationOptions = other.CompilationOptions;
        ParseOptions = other.ParseOptions;
    }

    public override string Language => LanguageNames.VisualBasic;

    internal override string DocumentName => "Test.vb";

    new public VisualBasicParseOptions ParseOptions { get; private set; }

    new public VisualBasicCompilationOptions CompilationOptions { get; private set; }

    protected override ParseOptions CommonParseOptions => ParseOptions;

    protected override CompilationOptions CommonCompilationOptions => CompilationOptions;

    public static VisualBasicTestOptions Default { get; } = CreateDefault();

    private static VisualBasicTestOptions CreateDefault()
    {
        VisualBasicParseOptions parseOptions = VisualBasicParseOptions.Default;

        var compilationOptions = new VisualBasicCompilationOptions(OutputKind.DynamicallyLinkedLibrary);

        return new VisualBasicTestOptions(
            compilationOptions: compilationOptions,
            parseOptions: parseOptions,
            metadataReferences: RuntimeMetadataReference.DefaultMetadataReferences.Select(f => f.Value).ToImmutableArray(),
            allowedCompilerDiagnosticIds: null,
            allowedCompilerDiagnosticSeverity: DiagnosticSeverity.Info);
    }

    public VisualBasicTestOptions AddMetadataReference(MetadataReference metadataReference)
    {
        return WithMetadataReferences(MetadataReferences.Add(metadataReference));
    }

    protected override TestOptions CommonWithMetadataReferences(IEnumerable<MetadataReference> values)
    {
        return new VisualBasicTestOptions(this) { MetadataReferences = values?.ToImmutableArray() ?? ImmutableArray<MetadataReference>.Empty };
    }

    protected override TestOptions CommonWithAllowedCompilerDiagnosticIds(IEnumerable<string> values)
    {
        return new VisualBasicTestOptions(this) { AllowedCompilerDiagnosticIds = values?.ToImmutableArray() ?? ImmutableArray<string>.Empty };
    }

    protected override TestOptions CommonWithAllowedCompilerDiagnosticSeverity(DiagnosticSeverity value)
    {
        return new VisualBasicTestOptions(this) { AllowedCompilerDiagnosticSeverity = value };
    }

    protected override TestOptions CommonWithConfigOptions(IEnumerable<KeyValuePair<string, string>> values)
    {
        return new VisualBasicTestOptions(this) { ConfigOptions = values?.ToImmutableDictionary() ?? ImmutableDictionary<string, string>.Empty };
    }

    new public VisualBasicTestOptions WithMetadataReferences(IEnumerable<MetadataReference> values)
    {
        return (VisualBasicTestOptions)base.WithMetadataReferences(values);
    }

    new public VisualBasicTestOptions WithAllowedCompilerDiagnosticIds(IEnumerable<string> values)
    {
        return (VisualBasicTestOptions)base.WithAllowedCompilerDiagnosticIds(values);
    }

    new public VisualBasicTestOptions WithAllowedCompilerDiagnosticSeverity(DiagnosticSeverity value)
    {
        return (VisualBasicTestOptions)base.WithAllowedCompilerDiagnosticSeverity(value);
    }

    public VisualBasicTestOptions WithParseOptions(VisualBasicParseOptions parseOptions)
    {
        return new VisualBasicTestOptions(this) { ParseOptions = parseOptions ?? throw new ArgumentNullException(nameof(parseOptions)) };
    }

    public VisualBasicTestOptions WithCompilationOptions(VisualBasicCompilationOptions compilationOptions)
    {
        return new VisualBasicTestOptions(this) { CompilationOptions = compilationOptions ?? throw new ArgumentNullException(nameof(compilationOptions)) };
    }

    new public VisualBasicTestOptions WithConfigOptions(IEnumerable<KeyValuePair<string, string>> values)
    {
        return (VisualBasicTestOptions)base.WithConfigOptions(values);
    }
}
