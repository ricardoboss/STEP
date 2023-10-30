using System.Drawing;

namespace StepLang.Tooling.Highlighting;

/// <summary>
/// Represents a style for a <see cref="StepLang.Tokenizing.TokenType"/>.
/// </summary>
/// <param name="Foreground">The foreground color.</param>
/// <param name="Italic">Whether the text should be italic.</param>
/// <param name="IsDefault">Whether this represents the default style.</param>
public record Style(Color? Foreground = null, bool Italic = false, bool IsDefault = false);
