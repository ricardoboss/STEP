using Leap.Common.API;

namespace Leap.API.Extensions;

public static class LibraryVersionExtensions
{
    public static BriefLibraryVersion ToBriefDto(this DB.Entities.LibraryVersion version, string downloadUrl)
    {
        return new(
            version.Library.Author,
            version.Library.Name,
            version.Version,
            version.Dependencies.ToDictionary(d => d.Dependency.Name, d => d.VersionRange),
            downloadUrl
        );
    }
}