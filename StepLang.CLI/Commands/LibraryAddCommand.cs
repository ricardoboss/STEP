using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using Semver;
using Spectre.Console.Cli;
using StepLang.Libraries;
using StepLang.Libraries.Client;

namespace StepLang.CLI.Commands;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes")]
internal sealed class LibraryAddCommand : AsyncCommand<LibraryAddCommand.Settings>
{
    public sealed class Settings : HiddenGlobalCommandSettings
    {
        [CommandArgument(0, "<name>")]
        [Description("The name of the library to add.")]
        public string Name { get; init; } = null!;

        [CommandArgument(1, "[version]")]
        [Description("The version or version range of the library to add.")]
        public string? VersionRange { get; init; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var libraryFilePath = Path.Combine(Directory.GetCurrentDirectory(), "library.json");
        if (!File.Exists(libraryFilePath))
            throw new InvalidOperationException("Not in a library");

        var libraryJson = await File.ReadAllTextAsync(libraryFilePath);
        var library = JsonSerializer.Deserialize<Library>(libraryJson) ?? throw new InvalidOperationException("Unable to read library.json");

        if (library.Dependencies?.ContainsKey(settings.Name) ?? false)
        {
            await Console.Out.WriteLineAsync($"A dependency on {settings.Name} already exists");
            await Console.Out.WriteAsync("Update existing dependency? [y/N] ");
            var response = await Console.In.ReadLineAsync();
            if (response?.Trim().ToLowerInvariant() != "y")
                return 0;
        }

        SemVersionRange semVersionRange;
        if (settings.VersionRange is null)
        {
            var latest = await GetLatestVersion(settings.Name);
            if (latest is null)
                throw new InvalidOperationException($"Unable to find a library named '{settings.Name}'");

            semVersionRange = SemVersionRange.AtLeast(latest);
        }
        else
        {
            semVersionRange = SemVersionRange.Parse(settings.VersionRange);
            var latestCompatible = await GetLatestVersionThatSatisfies(settings.Name, semVersionRange);
            if (latestCompatible is null)
                throw new InvalidOperationException($"Unable to find a library named '{settings.Name}' that satisfies '{semVersionRange}'");
        }

        var newLibrary = LibraryBuilder.From(library)
            .WithDependency(settings.Name, semVersionRange)
            .Build();

        await File.WriteAllTextAsync(libraryFilePath, JsonSerializer.Serialize(newLibrary, new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        }));

        await Console.Out.WriteLineAsync($"Successfully added '{settings.Name}' to library.json");

        return 0;
    }

    private static async Task<SemVersion?> GetLatestVersion(string name)
    {
        using var httpClient = new HttpClient();
        var apiClient = new LibApiClientFactory(null).CreateClient(httpClient);
        var briefLibrary = await apiClient.GetLibraryAsync(name);
        if (briefLibrary is null)
            return null;

        return SemVersion.Parse(briefLibrary.Version, SemVersionStyles.Strict);
    }

    private static async Task<SemVersion?> GetLatestVersionThatSatisfies(string name, SemVersionRange range)
    {
        using var httpClient = new HttpClient();
        var apiClient = new LibApiClientFactory(null).CreateClient(httpClient);
        var briefLibrary = await apiClient.GetLibraryAsync(name, range.ToString());
        if (briefLibrary is null)
            return null;

        return SemVersion.Parse(briefLibrary.Version, SemVersionStyles.Strict);
    }
}