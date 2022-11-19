﻿// Copyright (c) Josef Pihrt and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using CommandLine;

namespace Roslynator.CommandLine;

#if DEBUG
[Verb("list-references", HelpText = "Lists assembly references from the specified project or solution.")]
#endif
public class ListReferencesCommandLineOptions : MSBuildCommandLineOptions
{
    [Value(
        index: 0,
        HelpText = "Path to one or more project/solution files.",
        MetaName = "<PROJECT|SOLUTION>")]
    public IEnumerable<string> Paths { get; set; }

    [Option(
        longName: "display",
        HelpText = "Defines how the assembly is displayed. Allowed values are path (default), file-name, file-name-without-extension or assembly-name.")]
    public string Display { get; set; }

    [Option(
        longName: OptionNames.Type,
        HelpText = "Defines a type of a reference. Allowed values are dll and project.")]
    public IEnumerable<string> Type { get; set; }
}
