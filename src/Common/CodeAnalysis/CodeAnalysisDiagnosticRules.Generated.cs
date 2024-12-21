﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

// <auto-generated>

using System;
using Microsoft.CodeAnalysis;

namespace Roslynator.CodeAnalysis
{
    public static partial class CodeAnalysisDiagnosticRules
    {
        /// <summary>RCS9001</summary>
        public static readonly DiagnosticDescriptor UsePatternMatching = DiagnosticDescriptorFactory.Create(
            id:                 CodeAnalysisDiagnosticIdentifiers.UsePatternMatching, 
            title:              "Use pattern matching", 
            messageFormat:      "Use pattern matching", 
            category:           DiagnosticCategories.Roslynator, 
            defaultSeverity:    DiagnosticSeverity.Hidden, 
            isEnabledByDefault: true, 
            description:        null, 
            helpLinkUri:        CodeAnalysisDiagnosticIdentifiers.UsePatternMatching, 
            customTags:         []);

        /// <summary>RCS9002</summary>
        public static readonly DiagnosticDescriptor UsePropertySyntaxNodeSpanStart = DiagnosticDescriptorFactory.Create(
            id:                 CodeAnalysisDiagnosticIdentifiers.UsePropertySyntaxNodeSpanStart, 
            title:              "Use property SyntaxNode.SpanStart", 
            messageFormat:      "Use property SyntaxNode.SpanStart", 
            category:           DiagnosticCategories.Roslynator, 
            defaultSeverity:    DiagnosticSeverity.Info, 
            isEnabledByDefault: true, 
            description:        null, 
            helpLinkUri:        CodeAnalysisDiagnosticIdentifiers.UsePropertySyntaxNodeSpanStart, 
            customTags:         []);

        /// <summary>RCS9003</summary>
        public static readonly DiagnosticDescriptor UnnecessaryConditionalAccess = DiagnosticDescriptorFactory.Create(
            id:                 CodeAnalysisDiagnosticIdentifiers.UnnecessaryConditionalAccess, 
            title:              "Unnecessary conditional access", 
            messageFormat:      "Unnecessary conditional access", 
            category:           DiagnosticCategories.Roslynator, 
            defaultSeverity:    DiagnosticSeverity.Info, 
            isEnabledByDefault: true, 
            description:        null, 
            helpLinkUri:        CodeAnalysisDiagnosticIdentifiers.UnnecessaryConditionalAccess, 
            customTags:         WellKnownDiagnosticTags.Unnecessary);

        public static readonly DiagnosticDescriptor UnnecessaryConditionalAccessFadeOut = DiagnosticDescriptorFactory.CreateFadeOut(UnnecessaryConditionalAccess);

        /// <summary>RCS9004</summary>
        public static readonly DiagnosticDescriptor CallAnyInsteadOfAccessingCount = DiagnosticDescriptorFactory.Create(
            id:                 CodeAnalysisDiagnosticIdentifiers.CallAnyInsteadOfAccessingCount, 
            title:              "Call 'Any' instead of accessing 'Count'", 
            messageFormat:      "Call 'Any' instead of accessing 'Count'", 
            category:           DiagnosticCategories.Roslynator, 
            defaultSeverity:    DiagnosticSeverity.Info, 
            isEnabledByDefault: true, 
            description:        null, 
            helpLinkUri:        CodeAnalysisDiagnosticIdentifiers.CallAnyInsteadOfAccessingCount, 
            customTags:         []);

        /// <summary>RCS9005</summary>
        public static readonly DiagnosticDescriptor UnnecessaryNullCheck = DiagnosticDescriptorFactory.Create(
            id:                 CodeAnalysisDiagnosticIdentifiers.UnnecessaryNullCheck, 
            title:              "Unnecessary null check", 
            messageFormat:      "Unnecessary null check", 
            category:           DiagnosticCategories.Roslynator, 
            defaultSeverity:    DiagnosticSeverity.Info, 
            isEnabledByDefault: true, 
            description:        null, 
            helpLinkUri:        CodeAnalysisDiagnosticIdentifiers.UnnecessaryNullCheck, 
            customTags:         WellKnownDiagnosticTags.Unnecessary);

        /// <summary>RCS9006</summary>
        public static readonly DiagnosticDescriptor UseElementAccess = DiagnosticDescriptorFactory.Create(
            id:                 CodeAnalysisDiagnosticIdentifiers.UseElementAccess, 
            title:              "Use element access", 
            messageFormat:      "Use element access", 
            category:           DiagnosticCategories.Roslynator, 
            defaultSeverity:    DiagnosticSeverity.Info, 
            isEnabledByDefault: true, 
            description:        null, 
            helpLinkUri:        CodeAnalysisDiagnosticIdentifiers.UseElementAccess, 
            customTags:         []);

        /// <summary>RCS9007</summary>
        public static readonly DiagnosticDescriptor UseReturnValue = DiagnosticDescriptorFactory.Create(
            id:                 CodeAnalysisDiagnosticIdentifiers.UseReturnValue, 
            title:              "Use return value", 
            messageFormat:      "Use return value", 
            category:           DiagnosticCategories.Roslynator, 
            defaultSeverity:    DiagnosticSeverity.Warning, 
            isEnabledByDefault: true, 
            description:        null, 
            helpLinkUri:        CodeAnalysisDiagnosticIdentifiers.UseReturnValue, 
            customTags:         []);

        /// <summary>RCS9008</summary>
        public static readonly DiagnosticDescriptor CallLastInsteadOfUsingElementAccess = DiagnosticDescriptorFactory.Create(
            id:                 CodeAnalysisDiagnosticIdentifiers.CallLastInsteadOfUsingElementAccess, 
            title:              "Call 'Last' instead of using []", 
            messageFormat:      "Call 'Last' instead of using []", 
            category:           DiagnosticCategories.Roslynator, 
            defaultSeverity:    DiagnosticSeverity.Info, 
            isEnabledByDefault: true, 
            description:        null, 
            helpLinkUri:        CodeAnalysisDiagnosticIdentifiers.CallLastInsteadOfUsingElementAccess, 
            customTags:         []);

        /// <summary>RCS9009</summary>
        public static readonly DiagnosticDescriptor UnknownLanguageName = DiagnosticDescriptorFactory.Create(
            id:                 CodeAnalysisDiagnosticIdentifiers.UnknownLanguageName, 
            title:              "Unknown language name", 
            messageFormat:      "Unknown language name", 
            category:           DiagnosticCategories.Roslynator, 
            defaultSeverity:    DiagnosticSeverity.Warning, 
            isEnabledByDefault: true, 
            description:        null, 
            helpLinkUri:        CodeAnalysisDiagnosticIdentifiers.UnknownLanguageName, 
            customTags:         []);

        /// <summary>RCS9010</summary>
        public static readonly DiagnosticDescriptor SpecifyExportCodeRefactoringProviderAttributeName = DiagnosticDescriptorFactory.Create(
            id:                 CodeAnalysisDiagnosticIdentifiers.SpecifyExportCodeRefactoringProviderAttributeName, 
            title:              "Specify ExportCodeRefactoringProviderAttribute.Name", 
            messageFormat:      "Specify ExportCodeRefactoringProviderAttribute.Name", 
            category:           DiagnosticCategories.Roslynator, 
            defaultSeverity:    DiagnosticSeverity.Hidden, 
            isEnabledByDefault: true, 
            description:        null, 
            helpLinkUri:        CodeAnalysisDiagnosticIdentifiers.SpecifyExportCodeRefactoringProviderAttributeName, 
            customTags:         []);

        /// <summary>RCS9011</summary>
        public static readonly DiagnosticDescriptor SpecifyExportCodeFixProviderAttributeName = DiagnosticDescriptorFactory.Create(
            id:                 CodeAnalysisDiagnosticIdentifiers.SpecifyExportCodeFixProviderAttributeName, 
            title:              "Specify ExportCodeFixProviderAttribute.Name", 
            messageFormat:      "Specify ExportCodeFixProviderAttribute.Name", 
            category:           DiagnosticCategories.Roslynator, 
            defaultSeverity:    DiagnosticSeverity.Hidden, 
            isEnabledByDefault: true, 
            description:        null, 
            helpLinkUri:        CodeAnalysisDiagnosticIdentifiers.SpecifyExportCodeFixProviderAttributeName, 
            customTags:         []);

    }
}