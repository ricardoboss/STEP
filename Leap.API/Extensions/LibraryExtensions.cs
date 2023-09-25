using Leap.Common.API;

namespace Leap.API.Extensions;

public static class LibraryVersionExtensions
{
    public static BriefLibraryVersion ToBriefDto(this DB.Entities.LibraryVersion version, string downloadUrl)
    {
        return new(
            version.Library.Name,
            version.Version,
            version.Dependencies.Select(d => d.Name),
            downloadUrl
        );
    }
}