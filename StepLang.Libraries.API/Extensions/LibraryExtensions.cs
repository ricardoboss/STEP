namespace StepLang.Libraries.API.Extensions;

public static class LibraryVersionExtensions
{
    public static BriefLibraryDto ToBriefDto(this DB.Entities.LibraryVersion version)
    {
        return new(
            version.Library.Name,
            version.Version.ToSemVersion().ToDto(),
            version.Dependencies.Select(d => d.Name)
        );
    }
}