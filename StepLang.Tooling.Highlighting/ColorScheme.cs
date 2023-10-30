using System.Drawing;

namespace StepLang.Tooling.Highlighting;

/// <summary>
/// A color scheme for the syntax <see cref="Highlighter"/>.
/// </summary>
/// <param name="Default">The default style.</param>
/// <param name="Keyword">The style for keywords.</param>
/// <param name="Type">The style for types.</param>
/// <param name="Identifier">The style for identifiers.</param>
/// <param name="String">The style for strings.</param>
/// <param name="Number">The style for numbers.</param>
/// <param name="Bool">The style for booleans.</param>
/// <param name="Comment">The style for comments.</param>
/// <param name="Operator">The style for operators.</param>
/// <param name="Punctuation">The style for punctuation.</param>
public record ColorScheme(Style Default, Style Keyword, Style Type, Style Identifier, Style String, Style Number,
    Style Bool, Style Null, Style Comment,
    Style Operator, Style Punctuation)
{
    /// <summary>
    /// The names of the available color schemes.
    /// </summary>
    public static IEnumerable<string> Names => new[] { "Pale", "Dim", "Mono" };

    /// <summary>
    /// Gets a color scheme by name.
    /// </summary>
    /// <param name="name">The name of the color scheme.</param>
    /// <returns>The color scheme.</returns>
    /// <exception cref="NotSupportedException">Thrown if the color scheme is not supported.</exception>
    public static ColorScheme ByName(string name)
    {
        return name.ToLowerInvariant() switch
        {
            "pale" => Pale,
            "dim" => Dim,
            "mono" => Mono,
            _ => throw new NotSupportedException($"The color scheme '{name}' is not supported."),
        };
    }

    /// <summary>
    /// The pale color scheme for use in light mode.
    /// </summary>
    public static ColorScheme Pale { get; } = new(
        new(Color.White, IsDefault: true),
        new(Color.PaleVioletRed),
        new(Color.Turquoise),
        new(Color.PaleGoldenrod),
        new(Color.DarkSeaGreen),
        new(Color.Plum),
        new(Color.CadetBlue),
        new(Color.CadetBlue),
        new(Color.Gray, Italic: true),
        new(Color.White),
        new(Color.White)
    );

    /// <summary>
    /// The dim color scheme for use in dark mode.
    /// </summary>
    public static ColorScheme Dim { get; } = new(
        new(Color.Black, IsDefault: true),
        new(Color.MediumVioletRed),
        new(Color.DarkCyan),
        new(Color.DarkBlue),
        new(Color.Brown),
        new(Color.DarkGreen),
        new(Color.CadetBlue),
        new(Color.CadetBlue),
        new(Color.DarkGray, Italic: true),
        new(Color.Black),
        new(Color.Black)
    );

    /// <summary>
    /// The mono color scheme for.
    /// </summary>
    public static ColorScheme Mono { get; } = new(
        new(Color.Gray, IsDefault: true),
        new(Color.LightGray),
        new(Color.Gray),
        new(Color.DarkGray),
        new(Color.DarkGray, Italic: true),
        new(Color.DarkGray, Italic: true),
        new(Color.DarkGray, Italic: true),
        new(Color.DarkGray, Italic: true),
        new(Color.DimGray, Italic: true),
        new(Color.Gray),
        new(Color.Gray)
    );
}
