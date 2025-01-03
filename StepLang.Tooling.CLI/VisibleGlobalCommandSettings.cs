using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace StepLang.Tooling.CLI;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class VisibleGlobalCommandSettings : CommandSettings, IGlobalCommandSettings
{
	[CommandOption(IGlobalCommandSettings.InfoOptionName)]
	[Description(IGlobalCommandSettings.InfoOptionDescription)]
	[DefaultValue(IGlobalCommandSettings.InfoOptionDefaultValue)]
	public bool Info { get; init; }

	[CommandOption(IGlobalCommandSettings.VersionOptionName)]
	[Description(IGlobalCommandSettings.VersionOptionDescription)]
	[DefaultValue(IGlobalCommandSettings.VersionOptionDefaultValue)]
	public bool Version { get; init; }
}
