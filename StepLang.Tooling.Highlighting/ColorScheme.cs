using System.Drawing;

namespace StepLang.Tooling.Highlighting;

public record ColorScheme(Style Default, Style Keyword, Style Type, Style Identifier, Style String, Style Number,
    Style Bool, Style Comment,
    Style Operator, Style Punctuation)
{
    public static ColorScheme Bright { get; } = new(
        new(Color.White, Color.Transparent, false, false, false, false),
        new(Color.Yellow, Color.Transparent, false, false, false, false),
        new(Color.Cyan, Color.Transparent, false, false, false, false),
        new(Color.White, Color.Transparent, false, false, false, false),
        new(Color.Green, Color.Transparent, false, false, false, false),
        new(Color.Magenta, Color.Transparent, false, false, false, false),
        new(Color.Magenta, Color.Transparent, false, false, false, false),
        new(Color.Gray, Color.Transparent, false, false, false, false),
        new(Color.White, Color.Transparent, false, false, false, false),
        new(Color.White, Color.Transparent, false, false, false, false)
    );
}
