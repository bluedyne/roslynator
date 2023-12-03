﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Roslynator.CSharp.CSharpFactory;

namespace Roslynator.CSharp.Refactorings.WrapStatements;

internal sealed class WrapLinesInTryCatchRefactoring : WrapStatementsRefactoring<TryStatementSyntax>
{
    public const string Title = "try-catch";

    private WrapLinesInTryCatchRefactoring()
    {
    }

    public static WrapLinesInTryCatchRefactoring Instance { get; } = new();

    public override TryStatementSyntax CreateStatement(ImmutableArray<StatementSyntax> statements)
    {
        statements = statements.Replace(statements[0], statements[0].WithNavigationAnnotation());

        return TryStatement(
            Block(List(statements)),
            CatchClause(
                CatchDeclaration(
                    ParseTypeName("System.Exception").WithSimplifierAnnotation(),
                    Identifier("ex")),
                default(CatchFilterClauseSyntax),
                Block()));
    }
}
