using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
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
        var libraryFilePath = Path.Combine(Directory.GetCurrentDirectory(), "library.json");
        if (File.Exists(libraryFilePath))
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

        var library = builder.Build();

        await File.WriteAllTextAsync(libraryFilePath,
            JsonSerializer.Serialize(library,
                new JsonSerializerOptions
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                }));

        await Console.Out.WriteLineAsync($"Created library '{name}' at '{libraryFilePath}'");

        return 0;
    }
}