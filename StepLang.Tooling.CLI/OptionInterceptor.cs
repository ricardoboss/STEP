using Spectre.Console;
using Spectre.Console.Cli;
using StepLang.Tooling.CLI.Widgets;
using StepLang.Tooling.Meta;
using System.Runtime.InteropServices;
using InformationTuple =
	(StepLang.Tooling.Meta.IGitVersionProvider GitVersionProvider, StepLang.Tooling.Meta.IBuildTimeProvider
	BuildTimeProvider);

namespace StepLang.Tooling.CLI;

public sealed class OptionInterceptor(
	IAnsiConsole console,
	IGitVersionProvider binaryGitVersionProvider,
	IReadOnlyDictionary<string, InformationTuple> componentInformationProviders
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
		console.WriteLine(binaryGitVersionProvider.FullSemVer);
	}

	private void HandleInfoOption()
	{
		var definitionList = new DefinitionList();

		foreach (var (componentName, (gitVersionProvider, buildTimeProvider)) in componentInformationProviders)
		{
			var componentHeader = new Markup($"[dim]{componentName}[/]");
			var componentGrid = BuildComponentInformation(buildTimeProvider, gitVersionProvider);

			definitionList.Items.Add(new DefinitionListItem(componentHeader, componentGrid));
		}

		var environmentHeader = new Markup("[dim]Environment[/]");
		var environmentGrid = BuildEnvironmentInformation();
		definitionList.Items.Add(new DefinitionListItem(environmentHeader, environmentGrid));

		console.Write(definitionList);
	}

	private static Grid BuildComponentInformation(IBuildTimeProvider buildTimeProvider,
		IGitVersionProvider gitVersionProvider)
	{
		var data = new Dictionary<string, string>
		{
			{ "Build Date", $"{buildTimeProvider.BuildTime:yyyy-MM-dd HH:mm:ss} UTC" },
			{ "Version", $"{gitVersionProvider.Sha} ({gitVersionProvider.FullSemVer})" },
			{ "Branch", gitVersionProvider.BranchName },
		};

		var infoGrid = new Grid().AddColumns(2);

		var headerStyle = new Style(decoration: Decoration.Bold);

		foreach (var (name, value) in data)
		{
			_ = infoGrid.AddRow(new Text(name, headerStyle).RightJustified(), new Text(value));
		}

		return infoGrid;
	}

	private static Grid BuildEnvironmentInformation()
	{
		var data = new Dictionary<string, string>
		{
			{ "CLR Version", Environment.Version.ToString() },
			{ "OS Version", $"{RuntimeInformation.OSDescription} ({RuntimeInformation.OSArchitecture})" },
		};

		var infoGrid = new Grid().AddColumns(2);

		var headerStyle = new Style(decoration: Decoration.Bold);

		foreach (var (name, value) in data)
		{
			_ = infoGrid.AddRow(new Text(name, headerStyle).RightJustified(), new Text(value));
		}

		return infoGrid;
	}
}
