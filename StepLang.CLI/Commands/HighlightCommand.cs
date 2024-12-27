using Spectre.Console;
using Spectre.Console.Cli;
using StepLang.CLI.Converter;
using StepLang.CLI.Widgets;
using StepLang.Tooling.CLI;
using StepLang.Tooling.Highlighting;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace StepLang.CLI.Commands;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
internal sealed class HighlightCommand : AsyncCommand<HighlightCommand.Settings>
{
	[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
	[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
	public sealed class Settings : HiddenGlobalCommandSettings
	{
		[CommandArgument(0, "[file]")]
		[Description("The path to a .step-file to highlight and print to the console.")]
		public string? File { get; init; } = null;

		[CommandOption("-t|--theme <theme>")]
		[DefaultValue("pale")]
		[Description("The color scheme to use.")]
		[TypeConverter(typeof(ColorSchemeConverter))]
		public ColorScheme Theme { get; init; } = null!;

		[CommandOption("-l|--list-themes")]
		[DefaultValue(false)]
		[Description("List all available themes.")]
		public bool ListThemes { get; init; }

		[CommandOption("-n|--no-line-numbers")]
		[DefaultValue(false)]
		[Description("Hide line numbers.")]
		public bool HideLineNumbers { get; init; }
	}

	public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
	{
		if (settings.ListThemes)
		{
			AnsiConsole.MarkupLine("Available themes:");

			foreach (var name in ColorScheme.Names)
			{
				AnsiConsole.MarkupLine($" - {name}");
			}

			return 0;
		}

		if (settings.File is null)
		{
			AnsiConsole.MarkupLine("[red]No file specified.[/]");

			return 1;
		}

		var scriptFile = new FileInfo(settings.File);

		var source = await File.ReadAllTextAsync(scriptFile.FullName);

		var code = new Code(source, settings.Theme, !settings.HideLineNumbers);

		AnsiConsole.Write(code);

		return 0;
	}
}
