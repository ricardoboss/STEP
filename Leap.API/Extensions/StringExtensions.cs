using Semver;

namespace Leap.API.Extensions;

public static class StringExtensions
{
    public static SemVersion ToSemVersion(this string version) => SemVersion.Parse(version, SemVersionStyles.Any);
}