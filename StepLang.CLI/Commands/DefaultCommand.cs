using Spectre.Console.Cli;
using StepLang.Tooling.CLI;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace StepLang.CLI.Commands;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
internal sealed class DefaultCommand : AsyncCommand<DefaultCommand.Settings>
{
	[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
	[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
	public sealed class Settings : CommandSettings, IGlobalCommandSettings
	{
		public const string DefaultProgramFileName = "program.step";

		[CommandArgument(0, $"[file={DefaultProgramFileName}]")]
		[Description("The path to a .step-file to run.")]
		[DefaultValue(DefaultProgramFileName)]
		public string? File { get; init; }

		[CommandOption("-w|--no-warn")]
		[Description("Don't emit warnings when processing the source code.")]
		public bool NoWarn { get; init; }

		[CommandOption("--info")]
		[Description("Print the version and system information. Add the output of this to bug reports.")]
		public bool Info { get; init; }

		[CommandOption("-v|--version")]
		[Description("Print the version number.")]
		public bool Version { get; init; }

		public bool Handled { get; set; }
	}

	public override async Task<int> ExecuteAsync(CommandContext context, Settings settings,
		CancellationToken cancellationToken)
	{
		if (settings is { Handled: true })
			return 0;

		var fileToRun = settings.File ?? Settings.DefaultProgramFileName;

		var runCommand = new RunCommand();

		return await runCommand.ExecuteAsync(
			context,
			new() { File = fileToRun, NoWarn = settings.NoWarn },
			cancellationToken
		);
	}
}
