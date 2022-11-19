﻿// Copyright (c) Josef Pihrt and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Text;
using Roslynator.CSharp.Refactorings;
using Roslynator.Testing.CSharp.Xunit;
using Roslynator.Testing.Text;

namespace Roslynator.Testing.CSharp;

public abstract class AbstractCSharpRefactoringVerifier : XunitRefactoringVerifier<RoslynatorCodeRefactoringProvider>
{
    public abstract string RefactoringId { get; }

    public override CSharpTestOptions Options => DefaultCSharpTestOptions.Value;

    public async Task VerifyRefactoringAsync(
        string source,
        string expectedSource,
        IEnumerable<string> additionalFiles = null,
        string equivalenceKey = null,
        TestOptions options = null,
        CancellationToken cancellationToken = default)
    {
        var code = TestCode.Parse(source);

        var expected = ExpectedTestState.Parse(expectedSource);

        var data = new RefactoringTestData(
            code.Value,
            code.Spans.OrderByDescending(f => f.Start).ToImmutableArray(),
            AdditionalFile.CreateRange(additionalFiles),
            equivalenceKey: equivalenceKey);

        await VerifyRefactoringAsync(
            data,
            expected,
            options,
            cancellationToken: cancellationToken);
    }

    public async Task VerifyRefactoringAsync(
        string source,
        string sourceData,
        string expectedData,
        IEnumerable<string> additionalFiles = null,
        string equivalenceKey = null,
        TestOptions options = null,
        CancellationToken cancellationToken = default)
    {
        var code = TestCode.Parse(source, sourceData, expectedData);

        var expected = ExpectedTestState.Parse(code.ExpectedValue);

        var data = new RefactoringTestData(
            code.Value,
            code.Spans.OrderByDescending(f => f.Start).ToImmutableArray(),
            AdditionalFile.CreateRange(additionalFiles),
            equivalenceKey: equivalenceKey);

        await VerifyRefactoringAsync(
            data,
            expected,
            options,
            cancellationToken: cancellationToken);
    }

    public async Task VerifyNoRefactoringAsync(
        string source,
        string equivalenceKey = null,
        TestOptions options = null,
        CancellationToken cancellationToken = default)
    {
        var code = TestCode.Parse(source);

        var data = new RefactoringTestData(
            code.Value,
            code.Spans,
            equivalenceKey: equivalenceKey);

        await VerifyNoRefactoringAsync(
            data,
            options,
            cancellationToken: cancellationToken);
    }
}
