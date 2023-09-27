using System.Diagnostics.CodeAnalysis;
using Leap.Client;
using Leap.Common;
using Semver;
using Spectre.Console;
using StepLang.CLI.Extensions;

namespace StepLang.CLI.Services;

[SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes")]
internal sealed class InteractiveLibraryManager
{
    private readonly LeapApiClient apiClient;

    public InteractiveLibraryManager(LeapApiClient apiClient)
    {
        this.apiClient = apiClient;
    }

    public async Task<bool> AddDependency(Library library, string? dependencyName, string? versionRange)
    {
        dependencyName ??= AnsiConsole.Ask<string>("Enter the name of the library to add:");

        try
        {
            Library.ValidateName(dependencyName);
        }
        catch (ValidationException ve)
        {
            AnsiConsole.MarkupLineInterpolated($"[red]Invalid library name: {ve.Message}[/]");

            return false;
        }

        if (library.Dependencies?.ContainsKey(dependencyName) ?? false)
        {
            AnsiConsole.MarkupLineInterpolated($"A dependency on '[green]{dependencyName}[/]' already exists.");
            if (!AnsiConsole.Confirm("Do you want to update it to the [blue]latest version[/]?"))
            {
                AnsiConsole.MarkupLine("[red]Aborting.[/]");

                return false;
            }
        }

        var authorPart = Library.GetAuthorPart(dependencyName);
        var libraryPart = Library.GetLibraryPart(dependencyName);

        SemVersionRange semVersionRange;
        if (versionRange is null)
        {
            var latest = await apiClient.GetLatestVersion(authorPart, libraryPart);
            if (latest is null)
            {
                AnsiConsole.MarkupLineInterpolated($"[red]Could not find {dependencyName}[/]");

                return false;
            }

            semVersionRange = SemVersionRange.AtLeast(latest);

            AnsiConsole.MarkupLineInterpolated($"Using the latest version as minimum: [blue]{latest}[/]");
        }
        else
        {
            semVersionRange = SemVersionRange.Parse(versionRange);
        }

        library.Dependencies ??= new();
        library.Dependencies[dependencyName] = semVersionRange.ToString()!;

        AnsiConsole.MarkupLineInterpolated($"Added dependency on [green]{dependencyName}[/] with version range [blue]{semVersionRange}[/]");

        return true;
    }
}