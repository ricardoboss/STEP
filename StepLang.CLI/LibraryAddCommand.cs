using System.Text.Json;
using Semver;
using StepLang.Libraries;
using StepLang.Libraries.Client;
using StepLang.Libraries.Client.Extensions;

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
        }

        var dependency = new Dependency(name, semVersionRange);

        var newLibrary = LibraryBuilder.From(library)
            .WithDependency(dependency)
            .Build();

        await File.WriteAllTextAsync(libraryFilePath, JsonSerializer.Serialize(newLibrary));

        await Console.Out.WriteLineAsync($"Successfully added '{name}' to library.json");

        return 0;
    }

    private static async Task<SemVersion?> GetLatestVersion(string name)
    {
        using var httpClient = new HttpClient();
        var apiClient = new LibApiClientFactory().CreateClient(httpClient);
        var briefLibrary = await apiClient.GetLibraryAsync(name);
        return briefLibrary?.Version.ToSemVersion();
    }
}