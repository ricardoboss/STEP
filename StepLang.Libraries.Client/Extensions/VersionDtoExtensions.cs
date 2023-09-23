using Semver;
using StepLang.Libraries.API;

namespace StepLang.Libraries.Client.Extensions;

public static class VersionDtoExtensions
{
    public static SemVersion ToSemVersion(this VersionDto versionDto)
    {
        return new(versionDto.Major, versionDto.Minor, versionDto.Patch);
    }
}