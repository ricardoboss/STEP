using System.ComponentModel;
using Spectre.Console.Cli;

namespace StepLang.CLI.Commands;

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