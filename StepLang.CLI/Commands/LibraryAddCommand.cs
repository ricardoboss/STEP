using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Leap.Client;
using Semver;
using Spectre.Console.Cli;
using Leap.Common;

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

    private readonly LibApiClient apiClient;

    public LibraryAddCommand(LibApiClient apiClient)
    {
        this.apiClient = apiClient;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var library = Library.FromCurrentDir();

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
                throw new InvalidOperationException(
                    $"Unable to find a library named '{settings.Name}' that satisfies '{semVersionRange}'");
        }

        library
            .Modify()
            .WithDependency(settings.Name, semVersionRange)
            .Build()
            .SaveToCurrentDir();

        await Console.Out.WriteLineAsync($"Successfully added '{settings.Name}' to library.json");

        return 0;
    }

    private async Task<SemVersion?> GetLatestVersion(string name)
    {
        var briefLibrary = await apiClient.GetLibraryAsync(name);
        if (briefLibrary is null)
            return null;

        return SemVersion.Parse(briefLibrary.Version, SemVersionStyles.Strict);
    }

    private async Task<SemVersion?> GetLatestVersionThatSatisfies(string name, SemVersionRange range)
    {
        var briefLibrary = await apiClient.GetLibraryAsync(name, range.ToString());
        if (briefLibrary is null)
            return null;

        return SemVersion.Parse(briefLibrary.Version, SemVersionStyles.Strict);
    }
}