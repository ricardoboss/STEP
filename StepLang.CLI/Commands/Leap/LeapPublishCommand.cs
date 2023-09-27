using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;
using Leap.Client;
using Leap.Common;
using Semver;
using Spectre.Console.Cli;
using StepLang.CLI.Settings;

namespace StepLang.CLI.Commands.Leap;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes")]
internal sealed class LeapPublishCommand : AsyncCommand<LeapPublishCommand.Settings>
{
    public sealed class Settings : HiddenGlobalCommandSettings
    {
    }

    private readonly LeapApiClient apiClient;

    public LeapPublishCommand(LeapApiClient apiClient)
    {
        this.apiClient = apiClient;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var library = Library.FromCurrentDir();

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
            .Build()
            .SaveToCurrentDir();

        // TODO implement .stepignore
        using var zipStream = new MemoryStream();
        using (var zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
        {
            // TODO use newLibrary.Files
            var files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var relativePath = Path.GetRelativePath(Directory.GetCurrentDirectory(), file);

                // TODO: consolidate file selection and ignore downloaded libraries directory
                if (relativePath.Contains("libraries")) continue;

                var entry = zipArchive.CreateEntry(relativePath);
                await using var entryStream = entry.Open();
                await using var fileStream = File.OpenRead(file);
                await fileStream.CopyToAsync(entryStream);
            }
        }

        zipStream.Position = 0;

        var result = await apiClient.UploadLibraryAsync(newLibrary.NameAuthorPart, newLibrary.NameLibraryPart, newLibrary.Version!, zipStream);
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