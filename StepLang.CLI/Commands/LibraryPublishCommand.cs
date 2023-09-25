using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;
using System.Text.Json;
using System.Text.Json.Serialization;
using Semver;
using Spectre.Console.Cli;
using StepLang.Libraries;
using StepLang.Libraries.Client;

namespace StepLang.CLI.Commands;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes")]
internal sealed class LibraryPublishCommand : AsyncCommand<LibraryPublishCommand.Settings>
{
    public sealed class Settings : HiddenGlobalCommandSettings
    {
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var libraryFilePath = Path.Combine(Directory.GetCurrentDirectory(), "library.json");
        if (!File.Exists(libraryFilePath))
            throw new InvalidOperationException("Not in a library");

        var libraryJson = await File.ReadAllTextAsync(libraryFilePath);
        var library = JsonSerializer.Deserialize<Library>(libraryJson) ?? throw new InvalidOperationException("Unable to read library.json");

        var defaultVersion = "";
        if (library.Version is { } version)
            defaultVersion = $" [{version}]";

        await Console.Out.WriteAsync($"New version{defaultVersion}: ");
        var newVersionString = await Console.In.ReadLineAsync();

        SemVersion newVersion;
        if (string.IsNullOrWhiteSpace(newVersionString))
        {
            if (library.Version is null)
                throw new InvalidOperationException("A version is required for the first publish.");

            newVersion = SemVersion.Parse(library.Version, SemVersionStyles.Strict);
        }
        else
        {
            if (!SemVersion.TryParse(newVersionString, SemVersionStyles.Strict, out newVersion))
                throw new InvalidOperationException($"Invalid version '{newVersionString}'");
        }

        var newLibrary = LibraryBuilder.From(library)
            .WithVersion(newVersion)
            .Build();

        await File.WriteAllTextAsync(libraryFilePath, JsonSerializer.Serialize(newLibrary, new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        }));

        // pack all files in the current directory, not ignored by .stepignore, into a single zip stream
        using var zipStream = new MemoryStream();
        using (var zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
        {
            var files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var relativePath = Path.GetRelativePath(Directory.GetCurrentDirectory(), file);
                var entry = zipArchive.CreateEntry(relativePath);
                await using var entryStream = entry.Open();
                await using var fileStream = File.OpenRead(file);
                await fileStream.CopyToAsync(entryStream);
            }
        }

        zipStream.Position = 0;

        using var httpClient = new HttpClient();
        var apiClient = new LibApiClientFactory(null).CreateClient(httpClient);

        var result = await apiClient.UploadLibraryAsync(newLibrary.Name, newLibrary.Version!, zipStream);
        if (result is null)
        {
            await Console.Error.WriteLineAsync("Failed to upload library.");

            return 1;
        }

        if (result.Code != "success")
        {
            await Console.Error.WriteLineAsync($"Failed to upload library: {result.Message} ({result.Code})");

            return 2;
        }

        await Console.Out.WriteLineAsync($"{result.Message}");
        await Console.Out.WriteLineAsync($"Successfully published version {newVersion} of {newLibrary.Name}!");

        return 0;
    }
}