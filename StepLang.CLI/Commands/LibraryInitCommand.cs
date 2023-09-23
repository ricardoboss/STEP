using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Spectre.Console.Cli;
using StepLang.Libraries;

namespace StepLang.CLI.Commands;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes")]
internal sealed class LibraryInitCommand : AsyncCommand<LibraryInitCommand.Settings>
{
    public sealed class Settings : HiddenGlobalCommandSettings
    {
        [CommandOption("-n|--name <name>")]
        [Description("The name of the library to create.")]
        public string? Name { get; init; }

        [CommandOption("-a|--author <author>")]
        [Description("The author of the library to create.")]
        public string? Author { get; init; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        if (Library.IsCurrentDirLibrary())
            throw new InvalidOperationException("library.json already exists");

        var builder = new LibraryBuilder();

        var name = settings.Name;
        if (name is null)
        {
            await Console.Out.WriteAsync("Name: ");
            name = Console.ReadLine() ?? throw new InvalidOperationException("Name must be set");
        }

        builder.WithName(name);

        await Console.Out.WriteAsync("Author [enter to skip]: ");
        var author = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(author))
            builder.WithAuthor(author);

        // add some defaults
        builder.WithVersion(new(0, 1, 0));
        builder.WithFiles(new List<string>());

        builder.Build().SaveToCurrentDir();

        await Console.Out.WriteLineAsync($"Created library '{name}'");

        return 0;
    }
}