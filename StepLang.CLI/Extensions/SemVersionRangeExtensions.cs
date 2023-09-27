using Semver;

namespace StepLang.CLI.Extensions;

internal static class SemVersionRangeExtensions
{
    public static FormattableString Markup(this SemVersionRange versionRange) => $"[blue]{versionRange}[/]";
}