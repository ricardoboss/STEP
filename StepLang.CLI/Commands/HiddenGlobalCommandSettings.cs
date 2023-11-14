using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Spectre.Console.Cli;

namespace StepLang.CLI.Commands;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
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