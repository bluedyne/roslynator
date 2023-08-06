﻿// Copyright (c) Josef Pihrt and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DotMarkdown;
using DotMarkdown.Docusaurus;
using static DotMarkdown.Linq.MFactory;

namespace Roslynator.CommandLine.Documentation;

internal static class Program
{
    private static void Main(params string[] args)
    {
        var application = new CommandLineApplication(
            "roslynator",
            "Roslynator Command-line Tool",
            CommandLoader.LoadCommands(typeof(CommandLoader).Assembly)
                .Select(c => c.WithOptions(c.Options.OrderBy(f => f, CommandOptionComparer.Name)))
                .OrderBy(c => c.Name, StringComparer.InvariantCulture));

        if (args.Length < 2)
        {
            Console.WriteLine("Invalid number of arguments");
            return;
        }

        string destinationDirectoryPath = args[0];
        string dataDirectoryPath = args[1];

        string[] ignoredCommandNames = (args.Length > 2)
            ? Regex.Split(args[2], ",")
            : Array.Empty<string>();

        destinationDirectoryPath = Path.GetFullPath(destinationDirectoryPath);
        dataDirectoryPath = Path.GetFullPath(dataDirectoryPath);

        Console.WriteLine($"Destination directory: {destinationDirectoryPath}");
        Console.WriteLine($"Data directory: {dataDirectoryPath}");

        var markdownFormat = new MarkdownFormat(
            bulletListStyle: BulletListStyle.Minus,
            tableOptions: MarkdownFormat.Default.TableOptions | TableOptions.FormatHeaderAndContent,
            angleBracketEscapeStyle: AngleBracketEscapeStyle.EntityRef);

        var settings = new MarkdownWriterSettings(markdownFormat);

        List<Command> commands = application.Commands.Where(f => !ignoredCommandNames.Contains(f.Name)).ToList();

        string filePath = Path.Combine(destinationDirectoryPath, "cli/commands.md");

        Directory.CreateDirectory(Path.GetDirectoryName(filePath));

        using (var sw = new StreamWriter(filePath, append: false, Encoding.UTF8))
        using (MarkdownWriter mw = MarkdownWriter.Create(sw, settings))
        using (var dw = new DocusaurusMarkdownWriter(mw))
        {
            WriteFrontMatter(dw, position: 0, label: "Commands");

            dw.WriteHeading1("Commands");

            Table(
                TableRow("Command", "Description"),
                commands.Select(f => TableRow(Link(f.Name, $"commands/{f.Name}.md"), f.Description)))
                .WriteTo(dw);

            Console.WriteLine(filePath);
        }

        foreach (Command command in commands)
        {
            string commandFilePath = Path.GetFullPath(Path.Combine(destinationDirectoryPath, "cli/commands", $"{command.Name}.md"));

            Directory.CreateDirectory(Path.GetDirectoryName(commandFilePath));

            using (var sw = new StreamWriter(commandFilePath, append: false, Encoding.UTF8))
            using (MarkdownWriter mw = MarkdownWriter.Create(sw, settings))
            using (var dw = new DocusaurusMarkdownWriter(mw))
            {
                var writer = new DocumentationWriter(dw);

                WriteFrontMatter(dw, label: command.Name);

                writer.WriteCommandHeading(command, application);
                writer.WriteCommandDescription(command);

                string additionalContentFilePath = Path.Combine(dataDirectoryPath, command.Name + "_bottom.md");

                string additionalContent = (File.Exists(additionalContentFilePath))
                    ? File.ReadAllText(additionalContentFilePath)
                    : "";

                writer.WriteCommandSynopsis(command, application);
                writer.WriteArguments(command.Arguments);
                writer.WriteOptions(command.Options);

                if (!string.IsNullOrEmpty(additionalContent))
                {
                    dw.WriteLine();
                    dw.WriteLine();
                    dw.WriteRaw(additionalContent);
                }

                Console.WriteLine(commandFilePath);
            }
        }

        Console.WriteLine("Done");

        if (Debugger.IsAttached)
            Console.ReadKey();
    }

    private static void WriteFrontMatter(DocusaurusMarkdownWriter mw, int? position = null, string label = null)
    {
        var labels = new List<(string, object)>();

        if (position is not null)
            labels.Add(("sidebar_position", position));

        if (label is not null)
            labels.Add(("sidebar_label", label));

        mw.WriteDocusaurusFrontMatter(labels);
    }
}
