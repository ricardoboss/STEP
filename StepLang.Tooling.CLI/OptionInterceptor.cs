using Spectre.Console;
using Spectre.Console.Cli;
using System.Runtime.InteropServices;

namespace StepLang.Tooling.CLI;

public sealed class OptionInterceptor(
	IAnsiConsole console,
	IGitVersionProvider gitVersionProvider,
	IBuildTimeProvider buildTimeProvider
) : ICommandInterceptor
{
	public void Intercept(CommandContext context, CommandSettings settings)
	{
		if (settings is not IGlobalCommandSettings globalSettings)
			return;

		if (globalSettings.Version)
		{
			HandleVersionOption();
		}
		else if (globalSettings.Info)
		{
			HandleInfoOption();
		}
	}

	private void HandleVersionOption()
	{
		console.WriteLine(gitVersionProvider.FullSemVer);
	}

	private void HandleInfoOption()
	{
		var data = new Dictionary<string, string>
		{
			{ "Build Date", $"{buildTimeProvider.BuildTimeUtc:yyyy-MM-dd HH:mm:ss} UTC" },
			{ "Version", $"{gitVersionProvider.Sha} ({gitVersionProvider.FullSemVer})" },
			{ "Branch", gitVersionProvider.BranchName },
			{ "CLR Version", Environment.Version.ToString() },
			{ "OS Version", $"{RuntimeInformation.OSDescription} ({RuntimeInformation.OSArchitecture})" },
		};

		var infoGrid = new Grid().AddColumns(2);

		var headerStyle = new Style(decoration: Decoration.Bold);

		foreach (var (name, value) in data)
		{
			_ = infoGrid.AddRow(new Text(name, headerStyle).RightJustified(), new Text(value));
		}

		console.Write(infoGrid);
	}
}
