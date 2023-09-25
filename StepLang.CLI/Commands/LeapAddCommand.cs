using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Leap.Client;
using Semver;
using Spectre.Console.Cli;
using Leap.Common;

namespace StepLang.CLI.Commands;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes")]
internal sealed class LeapAddCommand : AsyncCommand<LeapAddCommand.Settings>
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

    private readonly LeapApiClient apiClient;

    public LeapAddCommand(LeapApiClient apiClient)
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

        var parts = settings.Name.Split('/');
        if (parts.Length != 2)
        {
            await Console.Error.WriteLineAsync("Library names must have the form '<author>/<library>'");

            return 1;
        }

        var (author, name) = (parts[0], parts[1]);

        SemVersionRange semVersionRange;
        if (settings.VersionRange is null)
        {
            var latest = await GetLatestVersion(author, name);
            if (latest is null)
                throw new InvalidOperationException($"Unable to find a library named '{author}/{name}'");

            semVersionRange = SemVersionRange.AtLeast(latest);
        }
        else
        {
            semVersionRange = SemVersionRange.Parse(settings.VersionRange);
            var latestCompatible = await GetLatestVersionThatSatisfies(author, name, semVersionRange);
            if (latestCompatible is null)
                throw new InvalidOperationException(
                    $"Unable to find a library named '{author}/{name}' that satisfies '{semVersionRange}'");
        }

        library
            .Modify()
            .WithDependency(settings.Name, semVersionRange)
            .Build()
            .SaveToCurrentDir();

        await Console.Out.WriteLineAsync($"Successfully added '{author}/{name}' to library.json");

        return 0;
    }

    private async Task<SemVersion?> GetLatestVersion(string author, string name)
    {
        var briefLibrary = await apiClient.GetLibraryAsync(author, name);
        if (briefLibrary is null)
            return null;

        return SemVersion.Parse(briefLibrary.Version, SemVersionStyles.Strict);
    }

    private async Task<SemVersion?> GetLatestVersionThatSatisfies(string author, string name, SemVersionRange range)
    {
        var briefLibrary = await apiClient.GetLibraryAsync(author, name, range.ToString());
        if (briefLibrary is null)
            return null;

        return SemVersion.Parse(briefLibrary.Version, SemVersionStyles.Strict);
    }
}