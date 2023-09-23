using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Spectre.Console.Cli;

namespace StepLang.CLI.Commands;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes")]
internal sealed class DefaultCommand : AsyncCommand<DefaultCommand.Settings>
{
    public sealed class Settings : ShownGlobalCommandSettings
    {
        [CommandArgument(0, "[file]")]
        [Description("The path to a .step-file to run.")]
        [DefaultValue(null)]
        public string? File { get; init; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        if (settings.File == null)
            return 0;

        var runCommand = new RunCommand();

        return await runCommand.ExecuteAsync(context, new() { File = settings.File });
    }
}