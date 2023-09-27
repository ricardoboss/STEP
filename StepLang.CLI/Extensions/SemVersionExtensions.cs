using Semver;

namespace StepLang.CLI.Extensions;

internal static class SemVersionExtensions
{
    public static FormattableString Markup(this SemVersion semVersion) => $"[blue]{semVersion}[/]";
}