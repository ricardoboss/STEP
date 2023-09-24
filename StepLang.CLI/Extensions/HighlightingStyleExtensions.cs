using Spectre.Console;

namespace StepLang.CLI.Extensions;

internal static class HighlightingStyleExtensions
{
    public static Style ToSpectreStyle(this Tooling.Highlighting.Style style)
    {
        var decoration = Decoration.None;
        if (style.Italic)
            decoration |= Decoration.Italic;

        return new(style.Foreground.ToSpectreColor(), decoration: decoration);
    }

    public static Color ToSpectreColor(this System.Drawing.Color? color)
    {
        if (color is null or { A: 0 })
            return Color.Default;

        return new(color.Value.R, color.Value.G, color.Value.B);
    }
}