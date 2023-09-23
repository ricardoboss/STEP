using System.ComponentModel;
using Spectre.Console.Cli;

namespace StepLang.CLI.Commands;

public class HiddenGlobalCommandSettings : CommandSettings, IGlobalCommandSettings
{
    [CommandOption(IGlobalCommandSettings.InfoOptionName, IsHidden = true)]
    [Description(IGlobalCommandSettings.InfoOptionDescription)]
    [DefaultValue(IGlobalCommandSettings.InfoOptionDefaultValue)]
    public bool Info { get; init; }

    [CommandOption(IGlobalCommandSettings.VersionOptionName, IsHidden = true)]
    [Description(IGlobalCommandSettings.VersionOptionDescription)]
    [DefaultValue(IGlobalCommandSettings.VersionOptionDefaultValue)]
    public bool Version { get; init; }
}