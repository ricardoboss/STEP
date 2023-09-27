using Leap.Client;
using Semver;

namespace StepLang.CLI.Extensions;

internal static class LeapApiClientExtensions
{
    public static async Task<SemVersion?> GetLatestVersion(this LeapApiClient apiClient, string author, string name)
    {
        var library = await apiClient.GetLibraryAsync(author, name);
        if (library is null)
            return null;

        return SemVersion.Parse(library.Version, SemVersionStyles.Strict);
    }

    public static async Task<SemVersion?> GetLatestVersion(this LeapApiClient apiClient, string author, string name, SemVersionRange versionRange)
    {
        var library = await apiClient.GetLibraryAsync(author, name, versionRange.ToString());
        if (library is null)
            return null;

        return SemVersion.Parse(library.Version, SemVersionStyles.Strict);
    }
}