﻿// Copyright (c) Josef Pihrt and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp.Syntax;
using Roslynator.CSharp.SyntaxWalkers;

namespace Roslynator.CSharp.Analysis;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class RemoveRedundantAssignmentAnalyzer : BaseDiagnosticAnalyzer
{
    private static ImmutableArray<DiagnosticDescriptor> _supportedDiagnostics;

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
    {
        get
        {
            if (_supportedDiagnostics.IsDefault)
            {
                Immutable.InterlockedInitialize(
                    ref _supportedDiagnostics,
                    DiagnosticRules.RemoveRedundantAssignment,
                    DiagnosticRules.RemoveRedundantAssignmentFadeOut);
            }

            return _supportedDiagnostics;
        }
    }

    public override void Initialize(AnalysisContext context)
    {
        base.Initialize(context);

        context.RegisterSyntaxNodeAction(
            c =>
            {
                if (DiagnosticRules.RemoveRedundantAssignment.IsEffective(c))
                    AnalyzeLocalDeclarationStatement(c);
            },
            SyntaxKind.LocalDeclarationStatement);

        context.RegisterSyntaxNodeAction(
            c =>
            {
                if (DiagnosticRules.RemoveRedundantAssignment.IsEffective(c))
                    AnalyzeSimpleAssignment(c);
            },
            SyntaxKind.SimpleAssignmentExpression);
    }

    private static void AnalyzeLocalDeclarationStatement(SyntaxNodeAnalysisContext context)
    {
        var localDeclaration = (LocalDeclarationStatementSyntax)context.Node;

        if (localDeclaration.ContainsDiagnostics)
            return;

        if (localDeclaration.SpanOrTrailingTriviaContainsDirectives())
            return;

        if (localDeclaration.IsConst)
            return;

        SingleLocalDeclarationStatementInfo localInfo = SyntaxInfo.SingleLocalDeclarationStatementInfo(localDeclaration);

        if (!localInfo.Success)
            return;

        SimpleAssignmentStatementInfo assignmentInfo = SyntaxInfo.SimpleAssignmentStatementInfo(localDeclaration.NextStatement());

        if (!assignmentInfo.Success)
            return;

        if (assignmentInfo.Statement.ContainsDiagnostics)
            return;

        if (assignmentInfo.Statement.SpanOrLeadingTriviaContainsDirectives())
            return;

        if (assignmentInfo.Left is not IdentifierNameSyntax identifierName)
            return;

        string name = identifierName.Identifier.ValueText;

        if (!string.Equals(localInfo.IdentifierText, name, StringComparison.Ordinal))
            return;

        SemanticModel semanticModel = context.SemanticModel;
        CancellationToken cancellationToken = context.CancellationToken;

        if (semanticModel.GetSymbol(identifierName, cancellationToken) is not ILocalSymbol localSymbol)
            return;

        if (!SymbolEqualityComparer.Default.Equals(localSymbol, semanticModel.GetDeclaredSymbol(localInfo.Declarator, cancellationToken)))
            return;

        ExpressionSyntax value = localInfo.Value;

        if (value is not null)
        {
            ITypeSymbol typeSymbol = semanticModel.GetTypeSymbol(localInfo.Type, cancellationToken);

            if (typeSymbol is null)
                return;

            if (!semanticModel.IsDefaultValue(typeSymbol, value, cancellationToken))
                return;

            if (IsReferenced(localSymbol, assignmentInfo.Right, semanticModel, cancellationToken))
                return;
        }

        DiagnosticHelpers.ReportDiagnostic(context, DiagnosticRules.RemoveRedundantAssignment, localInfo.Identifier);

        if (value is not null)
        {
            DiagnosticHelpers.ReportNode(context, DiagnosticRules.RemoveRedundantAssignmentFadeOut, localInfo.Initializer);
            DiagnosticHelpers.ReportToken(context, DiagnosticRules.RemoveRedundantAssignmentFadeOut, assignmentInfo.OperatorToken);
        }

        DiagnosticHelpers.ReportToken(context, DiagnosticRules.RemoveRedundantAssignmentFadeOut, localDeclaration.SemicolonToken);
        DiagnosticHelpers.ReportNode(context, DiagnosticRules.RemoveRedundantAssignmentFadeOut, assignmentInfo.Left);
    }

    private static bool IsReferenced(
        ILocalSymbol localSymbol,
        SyntaxNode node,
        SemanticModel semanticModel,
        CancellationToken cancellationToken)
    {
        foreach (SyntaxNode descendantOrSelf in node.DescendantNodesAndSelf())
        {
            if (descendantOrSelf.IsKind(SyntaxKind.IdentifierName)
                && SymbolEqualityComparer.Default.Equals(semanticModel.GetSymbol((IdentifierNameSyntax)descendantOrSelf, cancellationToken), localSymbol))
            {
                return true;
            }
        }

        return false;
    }

    private static void AnalyzeSimpleAssignment(SyntaxNodeAnalysisContext context)
    {
        if (context.Node.ContainsDiagnostics)
            return;

        if (context.Node.SpanOrTrailingTriviaContainsDirectives())
            return;

        var assignment = (AssignmentExpressionSyntax)context.Node;

        SimpleAssignmentStatementInfo assignmentInfo = SyntaxInfo.SimpleAssignmentStatementInfo(assignment);

        if (!assignmentInfo.Success)
            return;

        if (assignmentInfo.Left is not IdentifierNameSyntax identifierName)
            return;

        StatementListInfo statementsInfo = SyntaxInfo.StatementListInfo(assignmentInfo.Statement);

        if (!statementsInfo.Success)
            return;

        int index = statementsInfo.IndexOf(assignmentInfo.Statement);

        if (index == statementsInfo.Count - 1)
            return;

        if (index > 0)
        {
            StatementSyntax previousStatement = statementsInfo[index - 1];

            SimpleAssignmentStatementInfo assignmentInfo2 = SyntaxInfo.SimpleAssignmentStatementInfo(previousStatement);

            if (assignmentInfo2.Success
                && assignmentInfo2.Left is IdentifierNameSyntax identifierName2
                && string.Equals(identifierName.Identifier.ValueText, identifierName2.Identifier.ValueText, StringComparison.Ordinal))
            {
                return;
            }
        }

        StatementSyntax nextStatement = statementsInfo[index + 1];

        if (nextStatement.SpanOrLeadingTriviaContainsDirectives())
            return;

        if (nextStatement is not ReturnStatementSyntax returnStatement)
            return;

        if (returnStatement.Expression?.WalkDownParentheses() is not IdentifierNameSyntax identifierName3)
            return;

        if (!string.Equals(identifierName.Identifier.ValueText, identifierName3.Identifier.ValueText, StringComparison.Ordinal))
            return;

        ISymbol symbol = context.SemanticModel.GetSymbol(identifierName, context.CancellationToken);

        switch (symbol?.Kind)
        {
            case SymbolKind.Local:
                {
                    break;
                }
            case SymbolKind.Parameter:
                {
                    if (((IParameterSymbol)symbol).RefKind != RefKind.None)
                        return;

                    break;
                }
            default:
                {
                    return;
                }
        }

        if (IsAssignedInsideAnonymousFunctionButDeclaredOutsideOfIt())
            return;

        bool result;
        RemoveRedundantAssignmentWalker walker = null;

        try
        {
            walker = RemoveRedundantAssignmentWalker.GetInstance();

            walker.Symbol = symbol;
            walker.SemanticModel = context.SemanticModel;
            walker.CancellationToken = context.CancellationToken;
            walker.Result = false;

            walker.Visit(assignmentInfo.Right);

            result = walker.Result;
        }
        finally
        {
            if (walker is not null)
                RemoveRedundantAssignmentWalker.Free(walker);
        }

        if (result)
            return;

        if (IsDeclaredInTryStatementOrCatchClauseAndReferencedInFinallyClause(context, assignmentInfo.Statement, symbol))
            return;

        DiagnosticHelpers.ReportDiagnostic(context, DiagnosticRules.RemoveRedundantAssignment, assignment);

        bool IsAssignedInsideAnonymousFunctionButDeclaredOutsideOfIt()
        {
            SyntaxNode declaringSyntax = null;
            SyntaxNode n = assignment.Parent;

            do
            {
                if (CSharpFacts.IsAnonymousFunctionExpression(n.Kind()))
                {
                    if (declaringSyntax is null)
                    {
                        declaringSyntax = symbol.GetSyntaxOrDefault();

                        Debug.Assert(declaringSyntax is not null, "");

                        if (declaringSyntax is null)
                            break;

                        SyntaxDebug.Assert(declaringSyntax.IsKind(SyntaxKind.VariableDeclarator, SyntaxKind.Parameter), declaringSyntax);
                    }

                    SyntaxNode n2 = declaringSyntax.Parent;

                    do
                    {
                        if (CSharpFacts.IsAnonymousFunctionExpression(n2.Kind()))
                            return !object.ReferenceEquals(n, n2);

                        if (n2 is MemberDeclarationSyntax)
                            break;

                        n2 = n2.Parent;
                    }
                    while (n2 is not null);

                    return true;
                }
                else if (n is MemberDeclarationSyntax)
                {
                    break;
                }

                n = n.Parent;
            }
            while (n is not null);

            return false;
        }
    }

    private static bool IsDeclaredInTryStatementOrCatchClauseAndReferencedInFinallyClause(
        SyntaxNodeAnalysisContext context,
        StatementSyntax statement,
        ISymbol symbol)
    {
        SyntaxNode node = statement.Parent;

        while (node is not null
            && node is not MemberDeclarationSyntax
            && !node.IsKind(SyntaxKind.FinallyClause))
        {
            if (node is TryStatementSyntax tryStatement)
            {
                BlockSyntax block = tryStatement.Finally?.Block;

                if (block is not null)
                {
                    ContainsLocalOrParameterReferenceWalker walker = null;

                    try
                    {
                        walker = ContainsLocalOrParameterReferenceWalker.GetInstance(symbol, context.SemanticModel, context.CancellationToken);

                        walker.VisitBlock(block);

                        if (walker.Result)
                            return true;
                    }
                    finally
                    {
                        if (walker is not null)
                            ContainsLocalOrParameterReferenceWalker.Free(walker);
                    }
                }
            }

            node = node.Parent;
        }

        return false;
    }

    private class RemoveRedundantAssignmentWalker : LocalOrParameterReferenceWalker
    {
        [ThreadStatic]
        private static RemoveRedundantAssignmentWalker _cachedInstance;

        private int _anonymousFunctionDepth;

        public bool Result { get; set; }

        public ISymbol Symbol { get; set; }

        public SemanticModel SemanticModel { get; set; }

        public CancellationToken CancellationToken { get; set; }

        protected override bool ShouldVisit => !Result;

        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {
            if (_anonymousFunctionDepth == 0)
                return;

            CancellationToken.ThrowIfCancellationRequested();

            if (string.Equals(node.Identifier.ValueText, Symbol.Name, StringComparison.Ordinal)
                && SymbolEqualityComparer.Default.Equals(SemanticModel.GetSymbol(node, CancellationToken), Symbol))
            {
                Result = true;
            }
        }

        public override void VisitAnonymousMethodExpression(AnonymousMethodExpressionSyntax node)
        {
            _anonymousFunctionDepth++;
            base.VisitAnonymousMethodExpression(node);
            _anonymousFunctionDepth--;
        }

        public override void VisitSimpleLambdaExpression(SimpleLambdaExpressionSyntax node)
        {
            _anonymousFunctionDepth++;
            base.VisitSimpleLambdaExpression(node);
            _anonymousFunctionDepth--;
        }

        public override void VisitParenthesizedLambdaExpression(ParenthesizedLambdaExpressionSyntax node)
        {
            _anonymousFunctionDepth++;
            base.VisitParenthesizedLambdaExpression(node);
            _anonymousFunctionDepth--;
        }

        public static RemoveRedundantAssignmentWalker GetInstance()
        {
            RemoveRedundantAssignmentWalker walker = _cachedInstance;

            if (walker is not null)
            {
                Debug.Assert(walker.Symbol is null);
                Debug.Assert(walker.SemanticModel is null);
                Debug.Assert(walker.CancellationToken == default);

                _cachedInstance = null;
                return walker;
            }

            return new RemoveRedundantAssignmentWalker();
        }

        public static void Free(RemoveRedundantAssignmentWalker walker)
        {
            walker.Symbol = null;
            walker.SemanticModel = null;
            walker.CancellationToken = default;

            _cachedInstance = walker;
        }
    }
}
