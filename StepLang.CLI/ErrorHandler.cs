using Spectre.Console;
using Spectre.Console.Cli;
using StepLang.CLI.Widgets;
using StepLang.Tooling.Analysis;
using StepLang.Tooling.CLI.Widgets;
using StepLang.Tooling.Highlighting;
using System.Diagnostics.CodeAnalysis;

namespace StepLang.CLI;

[ExcludeFromCodeCoverage]
internal static class ErrorHandler
{
	[RequiresDynamicCode("Calls Spectre.Console.AnsiConsole.WriteException(Exception, ExceptionFormats)")]
	public static void HandleException(Exception e, ITypeResolver? typeResolver)
	{
		if (e is StepLangException sle)
		{
			HandleStepLangException(sle);

			return;
		}

		AnsiConsole.WriteException(e);
	}

	private static void HandleStepLangException(StepLangException e)
	{
		AnsiConsole.Write(new Rule($"[red] {e.ErrorCode}: {e.GetType().Name} [/]")
			.LeftJustified()
			.RuleStyle("red dim"));

		AnsiConsole.Write(new Padder(new Text(e.Message)));

		if (e.Location is { } location)
		{
			var details = new DefinitionList();

			if (location.File?.Exists ?? false)
			{
				details.Items.Add(new DefinitionListItem(
					new Markup("[bold]File[/]"),
					new TextPath(location.File.FullName)
				));
			}

			details.Items.Add(new DefinitionListItem(
				new Markup("[bold]Location[/]"),
				new Text($"line {location.Line}, column {location.Column}")
			));

			AnsiConsole.Write(details);

			if (location.File?.Exists ?? false)
			{
				const int contextLineCount = 5;

				var lines = File.ReadAllText(location.File.FullName).SplitLines().ToArray();
				var contextStartLine = Math.Max(0, location.Line - 1 - contextLineCount);
				var contextEndLine = Math.Min(lines.Length, location.Line + contextLineCount);
				var contextLines = lines[contextStartLine..contextEndLine];
				var context = string.Join(Environment.NewLine, contextLines);

				var code = new Code(context, ColorScheme.Pale, true, contextStartLine, location.Line, location.Column,
					location.Length);

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
		{
			AnsiConsole.Write(new Padder(new Markup($"[aqua]Tip[/]: {helpText}"), new Padding(0, 1, 0, 0)));
		}

		if (e.HelpLink is { } helpLink)
		{
			AnsiConsole.Write(
				new Padder(new Markup($"[aqua]See also[/]: [link]{helpLink}[/]"), new Padding(0, 1, 0, 0)));
		}

		AnsiConsole.WriteLine();
	}
}
