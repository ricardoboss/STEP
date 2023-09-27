using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Spectre.Console;
using Spectre.Console.Cli;
using StepLang.CLI.Settings;

namespace StepLang.CLI.Commands;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes")]
internal sealed class DefaultCommand : AsyncCommand<DefaultCommand.Settings>
{
    public sealed class Settings : VisibleGlobalCommandSettings
    {
        [CommandArgument(0, "[file]")]
        [Description("The path to a .step-file to run.")]
        [DefaultValue(null)]
        public string? File { get; init; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        if (settings.File == null)
        {
            if (settings is not { Info: false, Version: false })
                return 0;

            AnsiConsole.MarkupLine("[red]No file specified.[/]");

            return 1;
        }

        var runCommand = new RunCommand();

        return await runCommand.ExecuteAsync(context, new() { File = settings.File });
    }
}