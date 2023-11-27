﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using CommandLine;

namespace Roslynator.CommandLine;

[Verb("find-symbol", HelpText = "Finds symbols in the specified project or solution.")]
public class FindSymbolCommandLineOptions : MSBuildCommandLineOptions
{
    [Value(
        index: 0,
        HelpText = "Path to one or more project/solution files.",
        MetaName = "<PROJECT|SOLUTION>")]
    public IEnumerable<string> Paths { get; set; }

    [Option(longName: "ignore-generated-code")]
    public bool IgnoreGeneratedCode { get; set; }

    [Option(
        longName: "remove",
        HelpText = "Remove found symbols' declarations.")]
    public bool Remove { get; set; }

    [Option(
        longName: OptionNames.SymbolKind,
        HelpText = "Space separated list of symbol kinds to be included. "
            + "Allowed values are class, delegate, enum, interface, struct, event, field, enum-field, const, method, property, indexer, member and type.")]
    public IEnumerable<string> SymbolKind { get; set; }

    [Option(
        longName: "unused",
        HelpText = "Search only for symbols that have zero references.")]
    public bool Unused { get; set; }

    [Option(
        longName: OptionNames.Visibility,
        HelpText = "Space separated list of visibilities of a type or a member. Allowed values are public, internal and private.",
        MetaValue = "<VISIBILITY>")]
    public IEnumerable<string> Visibility { get; set; }

    [Option(
        longName: "with-attribute",
        HelpText = "Space separated list of attributes that should be included.",
        MetaValue = "<METADATA_NAME>")]
    public IEnumerable<string> WithAttribute { get; set; }

    [Option(
        longName: "without-attribute",
        HelpText = "Space separated list of attributes that should be excluded.",
        MetaValue = "<METADATA_NAME>")]
    public IEnumerable<string> WithoutAttribute { get; set; }
}
