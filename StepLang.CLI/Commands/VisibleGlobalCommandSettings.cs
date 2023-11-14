using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Spectre.Console.Cli;

namespace StepLang.CLI.Commands;

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