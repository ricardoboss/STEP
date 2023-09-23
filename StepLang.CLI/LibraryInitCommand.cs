using System.Text.Json;
using System.Text.Json.Serialization;
using Semver;
using StepLang.Libraries;

namespace StepLang.CLI;

public static class LibraryInitCommand
{
    public static async Task<int> Invoke(string? name)
    {
        var libraryFilePath = Path.Combine(Directory.GetCurrentDirectory(), "library.json");
        if (File.Exists(libraryFilePath))
            throw new InvalidOperationException("library.json already exists");

        var builder = new LibraryBuilder();

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
                    WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                }));

        await Console.Out.WriteLineAsync($"Created library '{name}' at '{libraryFilePath}'");

        return 0;
    }
}