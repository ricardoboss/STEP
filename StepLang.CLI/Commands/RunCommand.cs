using Spectre.Console;
using Spectre.Console.Cli;
using StepLang.Diagnostics;
using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Tokenizing;
using StepLang.Tooling.CLI.Widgets;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace StepLang.CLI.Commands;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
internal sealed class RunCommand : AsyncCommand<RunCommand.Settings>
{
	public sealed class Settings : CommandSettings
	{
		[CommandArgument(0, "<file>")]
		[Description("The path to a .step-file to run.")]
		public string File { get; init; } = null!;

		[CommandOption("-w|--no-warn")]
		[Description("Don't emit warnings when processing the source code.")]
		public bool NoWarn { get; init; }
	}

	public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
	{
		var scriptFile = new FileInfo(settings.File);
		if (!scriptFile.Exists)
		{
			if (settings.File.Equals(DefaultCommand.Settings.DefaultProgramFileName))
			{
				AnsiConsole.MarkupLine($"[red]No [bold]{DefaultCommand.Settings.DefaultProgramFileName}[/] found.[/]");

				return 1;
			}

			AnsiConsole.MarkupLine($"[red]File [bold]{settings.File}[/] not found.[/]");

			return 1;
		}

		var source = await CharacterSource.FromFileAsync(scriptFile);

		var diagnostics = new DiagnosticCollection();
		if (!settings.NoWarn)
		{
			diagnostics.CollectionChanged += (_, e) =>
			{
				if (e is { Action: NotifyCollectionChangedAction.Add, NewItems: not null })
				{
					ReportDiagnostics(e.NewItems);
				}
			};
		}

		var tokenizer = new Tokenizer(source, diagnostics);
		var tokens = tokenizer.Tokenize();

		var parser = new Parser(tokens, diagnostics);
		var root = parser.ParseRoot();

		if (diagnostics.ContainsErrors)
		{
			AnsiConsole.MarkupLine("[bold red]The file contains errors:[/]");

			ReportDiagnostics(diagnostics.Errors);

			return -1;
		}

		var interpreter = new Interpreter(Console.Out, Console.Error, Console.In, null, diagnostics);
		root.Accept(interpreter);

		return interpreter.ExitCode;
	}

	private static void ReportDiagnostics(IEnumerable<Diagnostic> diagnostics)
	{
		foreach (var diagnostic in diagnostics)
		{
			var line = new DiagnosticLine(diagnostic);

			AnsiConsole.Write(line);
		}
	}
}
