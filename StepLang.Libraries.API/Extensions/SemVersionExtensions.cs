using Semver;

namespace StepLang.Libraries.API.Extensions;

public static class SemVersionExtensions
{
    public static VersionDto ToDto(this SemVersion version)
    {
        return new(
            version.Major,
            version.Minor,
            version.Patch
        );
    }
}