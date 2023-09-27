using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;
using Leap.Client;
using Leap.Common;
using Leap.Common.API;
using Spectre.Console;
using Spectre.Console.Cli;
using StepLang.CLI.Settings;

namespace StepLang.CLI.Commands.Leap;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes")]
internal sealed class LeapInstallCommand : AsyncCommand<LeapInstallCommand.Settings>
{
    public sealed class Settings : HiddenGlobalCommandSettings
    {
        [CommandOption("-n|--no-cache")]
        [Description("Whether to always download dependencies.")]
        [DefaultValue(false)]
        public bool NoCache { get; set; }
    }

    private readonly HttpClient httpClient;
    private readonly LeapApiClient apiClient;

    public LeapInstallCommand(LeapApiClient apiClient, HttpClient httpClient)
    {
        this.apiClient = apiClient;
        this.httpClient = httpClient;
    }

    /// <inheritdoc />
    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var library = Library.FromCurrentDir();
        var dependencies = new HashSet<BriefLibraryVersion>();

        AnsiConsole.MarkupLineInterpolated($"Resolving dependencies...");

        await ResolveDirectDependencies(dependencies, library.Dependencies ?? new Dictionary<string, string>());

        AnsiConsole.MarkupLineInterpolated($"Resolved {dependencies.Count} dependencies");

        var cacheDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".leap_cache");
        var dependenciesDir = Path.Combine(Directory.GetCurrentDirectory(), "libraries");

        var progress = new Progress(AnsiConsole.Console);
        await progress.StartAsync(async progressContext => await Parallel.ForEachAsync(dependencies, async (dependency, ct) =>
            {
                var dependencyName = $"{dependency.Author}/{dependency.Name}@{dependency.Version}";
                var progressTask = progressContext.AddTask(dependencyName, maxValue: 2);
                progressTask.StartTask();

                var dependencyCacheDir = Path.Combine(cacheDir, dependency.Author, dependency.Name, dependency.Version);
                var dependencyLibraryDir = Path.Combine(dependenciesDir, dependency.Author, dependency.Name);
                var dependencyLibraryZip = Path.Combine(dependencyCacheDir, "library.zip");

                if (!File.Exists(dependencyLibraryZip) || settings.NoCache)
                {
                    Directory.CreateDirectory(dependencyCacheDir);

                    var downloadUri = new Uri(dependency.DownloadUrl);
                    await using var downloadStream = await httpClient.GetStreamAsync(downloadUri, ct);
                    await using var fileStream = File.OpenWrite(dependencyLibraryZip);

                    // TODO: report download progress

                    await downloadStream.CopyToAsync(fileStream, ct);
                }

                progressTask.Increment(1);

                Directory.CreateDirectory(dependencyLibraryDir);

                await using var archiveFileStream = File.OpenRead(dependencyLibraryZip);
                using var zipStream = new ZipArchive(archiveFileStream);

                zipStream.ExtractToDirectory(dependencyLibraryDir, true);

                progressTask.Increment(1);
                progressTask.StopTask();

                AnsiConsole.MarkupLineInterpolated($"[bold]{dependencyName}[/] installed");
            })
        );

        return 0;
    }

    private async Task ResolveDirectDependencies(ISet<BriefLibraryVersion> resolved, IDictionary<string, string> dependencies)
    {
        foreach (var dependency in dependencies)
            await ResolveDirectDependencies(resolved, dependency.Key, dependency.Value);
    }

    private async Task ResolveDirectDependencies(ISet<BriefLibraryVersion> resolved, string libraryName, string versionRange)
    {
        var nameParts = libraryName.Split('/');
        var (author, name) = (nameParts[0], nameParts[1]);

        var briefLibraryVersion = await apiClient.GetLibraryAsync(author, name, versionRange);
        if (briefLibraryVersion is null)
        {
            AnsiConsole.MarkupLineInterpolated($"Failed to resolve dependency {author}/{name}@{versionRange}");

            return;
        }

        resolved.Add(briefLibraryVersion);

        await ResolveDirectDependencies(resolved, briefLibraryVersion.Dependencies);
    }
}