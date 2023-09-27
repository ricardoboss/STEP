using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Leap.Common;
using Spectre.Console.Cli;
using StepLang.CLI.Services;
using StepLang.CLI.Settings;

namespace StepLang.CLI.Commands.Leap;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes")]
internal sealed class LeapAddCommand : AsyncCommand<LeapAddCommand.Settings>
{
    public sealed class Settings : HiddenGlobalCommandSettings
    {
        [CommandArgument(0, "[name]")]
        [Description("The name of the library to add.")]
        public string? Name { get; init; }

        [CommandArgument(1, "[version]")]
        [Description("The version or version range of the library to add.")]
        public string? VersionRange { get; init; }
    }

    private readonly InteractiveLibraryManager libraryManager;

    public LeapAddCommand(InteractiveLibraryManager libraryManager)
    {
        this.libraryManager = libraryManager;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var library = Library.FromCurrentDir();

        if (!await libraryManager.AddDependency(library, settings.Name, settings.VersionRange))
            return 1;

        library.SaveToCurrentDir();

        return 0;
    }
}