using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.Console.Rendering;
using StepLang.CLI.Widgets;
using StepLang.Tooling.Formatting;
using StepLang.Tooling.Highlighting;

namespace StepLang.CLI;

[ExcludeFromCodeCoverage]
internal static partial class ErrorHandler
{
    public static void HandleException(Exception e)
    {
        if (e is StepLangException sle)
        {
            HandleStepLangException(sle);

            return;
        }

        HandleGeneralException(e);
    }

    private static IRenderable RenderExceptionHeader(string title, string message)
    {
        return new Rows(
            new Rule(title)
                .LeftJustified()
                .RuleStyle("red dim"),
            new Padder(new Text(message), new(2, 1, 0, 1))
        );
    }

    private static IEnumerable<TreeNode> RenderStackTrace(string stack)
    {
        var r = StackTraceLineRegex();
        return stack
            .SplitLines()
            .Select(l => r.Match(l))
            .Select(m =>
            {
                if (!m.Success)
                    return new(new Markup($"[red]{m.Value}[/]"));

                var method = m.Groups["method"].Value;
                var file = m.Groups["file"].Value;
                var line = m.Groups["line"].Value;

                var methodParts = method.Split('.');
                var methodSignature = methodParts[^1];
                var typeName = methodParts.Length > 1 ? methodParts[^2] : "???";
                // var namespaceName = string.Join('.', methodParts[..^2]);

                var node = new TreeNode(new Markup($"[#bbbbbb]{typeName}[/].[bold]{methodSignature}[/]"));
                node.AddNode(
                    new Grid()
                        .AddColumns(2)
                        .AddRow(
                            new Markup("[#bbbbbb]in [/]"),
                            new TextPath(file).LeafStyle("green")
                        )
                );

                node.AddNode(
                    new Markup($"[#bbbbbb]line[/] [yellow]{line}[/]")
                );

                return node;
            });
    }

    private static void HandleGeneralException(Exception e)
    {
        AnsiConsole.Write(RenderExceptionHeader(e.GetType().Name, e.Message));

        if (e is CommandAppException { Pretty: { } pretty })
        {
            AnsiConsole.Write(pretty);
        }
        else if (e.StackTrace is { } stack)
        {
            var tree = new Tree("Stack Trace")
                .Guide(TreeGuide.Line)
                .Style("grey");

            tree.AddNodes(RenderStackTrace(stack));

            AnsiConsole.Write(tree);
        }
    }

    private static void HandleStepLangException(StepLangException e)
    {
        AnsiConsole.Write(RenderExceptionHeader($"{e.ErrorCode}: {e.GetType().Name}", e.Message));

        if (e.Location is { } location)
        {
            var details = new DefinitionList();

            if (location.File.Exists)
            {
                details.Items.Add(new(
                    new Markup("[bold]File[/]"),
                    new TextPath(location.File.FullName)
                ));
            }

            details.Items.Add(new(
                new Markup("[bold]Location[/]"),
                new Text($"line {location.Line}, column {location.Column}")
            ));

            AnsiConsole.Write(details);

            if (location.File.Exists)
            {
                const int contextLineCount = 5;

                var lines = File.ReadAllText(location.File.FullName).SplitLines().ToArray();
                var contextStartLine = Math.Max(0, location.Line - 1 - contextLineCount);
                var contextEndLine = Math.Min(lines.Length, location.Line + contextLineCount);
                var contextLines = lines[contextStartLine..contextEndLine];
                var context = string.Join(Environment.NewLine, contextLines);

                var code = new Code(context, ColorScheme.Pale, true, contextStartLine, location.Line);

                var codePanel = new Panel(code)
                        .Header("[bold]Code[/]", Justify.Left)
                        .RoundedBorder()
                        .BorderStyle("grey")
                        .Padding(1, 0, 2, 0)
                    ;

                AnsiConsole.Write(codePanel);
            }
        }

        if (e.HelpText is { } helpText)
            AnsiConsole.Write(new Padder(new Markup($"[aqua]Tip[/]: {helpText}"), new(0, 1, 0, 0)));

        if (e.HelpLink is { } helpLink)
            AnsiConsole.Write(new Padder(new Markup($"[aqua]See also[/]: [link]{helpLink}[/]"), new(0, 1, 0, 0)));

        AnsiConsole.WriteLine();
    }

    [GeneratedRegex(@"^\s*at (?<method>.*) in (?<file>.*)\:line (?<line>\d+)$")]
    private static partial Regex StackTraceLineRegex();
}