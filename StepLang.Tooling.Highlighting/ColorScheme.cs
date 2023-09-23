using System.Drawing;

namespace StepLang.Tooling.Highlighting;

public record ColorScheme(Style Default, Style Keyword, Style Type, Style Identifier, Style String, Style Number,
    Style Bool, Style Comment,
    Style Operator, Style Punctuation)
{
    public static ColorScheme Bright { get; } = new(
        new(Color.White, Color.Transparent, false, false, false, false),
        new(Color.PaleVioletRed, Color.Transparent, false, false, false, false),
        new(Color.Turquoise, Color.Transparent, false, false, false, false),
        new(Color.Beige, Color.Transparent, false, false, false, false),
        new(Color.DarkSeaGreen, Color.Transparent, false, false, false, false),
        new(Color.Yellow, Color.Transparent, false, false, false, false),
        new(Color.CadetBlue, Color.Transparent, false, false, false, false),
        new(Color.Gray, Color.Transparent, false, false, false, false),
        new(Color.White, Color.Transparent, false, false, false, false),
        new(Color.White, Color.Transparent, false, false, false, false)
    );
}
