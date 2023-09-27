using Leap.Common;

namespace StepLang.CLI.Extensions;

internal static class LibraryExtensions
{
    public static FormattableString MarkupName(this Library library) => $"[green]{library.Name}[/]";
}