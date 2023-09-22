using System.Text.Json;
using System.Text.Json.Serialization;
using Semver;
using StepLang.Libraries;
using StepLang.Libraries.Client;

namespace StepLang.CLI;

public static class LibraryAddCommand
{
    public static async Task<int> Invoke(string name, string? versionRange)
    {
        var libraryFilePath = Path.Combine(Directory.GetCurrentDirectory(), "library.json");
        if (!File.Exists(libraryFilePath))
            throw new InvalidOperationException("Not in a library");

        var libraryJson = await File.ReadAllTextAsync(libraryFilePath);
        var library = JsonSerializer.Deserialize<Library>(libraryJson) ?? throw new InvalidOperationException("Unable to read library.json");

        if (library.Dependencies?.ContainsKey(name) ?? false)
        {
            await Console.Out.WriteLineAsync($"A dependency on {name} already exists");
            await Console.Out.WriteAsync("Update existing dependency? [y/N] ");
            var response = await Console.In.ReadLineAsync();
            if (response?.Trim().ToLowerInvariant() != "y")
                return 0;
        }

        SemVersionRange semVersionRange;
        if (versionRange is null)
        {
            var latest = await GetLatestVersion(name);
            if (latest is null)
                throw new InvalidOperationException($"Unable to find a library named '{name}'");

            semVersionRange = SemVersionRange.AtLeast(latest);
        }
        else
        {
            semVersionRange = SemVersionRange.Parse(versionRange);
            var latestCompatible = await GetLatestVersionThatSatisfies(name, semVersionRange);
            if (latestCompatible is null)
                throw new InvalidOperationException($"Unable to find a library named '{name}' that satisfies '{semVersionRange}'");
        }

        var newLibrary = LibraryBuilder.From(library)
            .WithDependency(name, semVersionRange)
            .Build();

        await File.WriteAllTextAsync(libraryFilePath, JsonSerializer.Serialize(newLibrary, new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        }));

        await Console.Out.WriteLineAsync($"Successfully added '{name}' to library.json");

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