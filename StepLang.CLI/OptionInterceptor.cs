using System.Runtime.InteropServices;
using cmdwtf;
using Spectre.Console;
using Spectre.Console.Cli;
using StepLang.CLI.Commands;

namespace StepLang.CLI;

internal sealed class OptionInterceptor : ICommandInterceptor
{
	public void Intercept(CommandContext context, CommandSettings settings)
	{
		if (settings is not IGlobalCommandSettings globalSettings)
		{
			return;
		}

		if (globalSettings.Version)
		{
			HandleVersionOption();
		}
		else if (globalSettings.Info)
		{
			HandleInfoOption();
		}
	}

	private static void HandleVersionOption()
	{
		AnsiConsole.WriteLine(GitVersionInformation.FullSemVer);
	}

	private static void HandleInfoOption()
	{
		var data = new Dictionary<string, string>
		{
			{ "Build Date", $"{BuildTimestamp.BuildTimeUtc:yyyy-MM-dd HH:mm:ss} UTC" },
			{ "Version", $"{GitVersionInformation.Sha} ({GitVersionInformation.FullSemVer})" },
			{ "Branch", GitVersionInformation.BranchName },
			{ "CLR Version", Environment.Version.ToString() },
			{ "OS Version", $"{RuntimeInformation.OSDescription} ({RuntimeInformation.OSArchitecture})" },
		};

		var infoGrid = new Grid().AddColumns(2);

		var headerStyle = new Style(decoration: Decoration.Bold);

		foreach (var (name, value) in data)
		{
			_ = infoGrid.AddRow(new Text(name, headerStyle).RightJustified(), new Text(value));
		}

		AnsiConsole.Write(infoGrid);
	}
}
