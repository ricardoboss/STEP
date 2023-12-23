using StepLang.Tokenizing;

namespace StepLang.Tooling.Highlighting;

/// <summary>
/// A token with a style.
/// </summary>
/// <param name="Type">The type of the token.</param>
/// <param name="Text">The content of the token.</param>
/// <param name="Style">The style of the token.</param>
/// <seealso cref="TokenType"/>
public record StyledToken(TokenType Type, string Text, Style Style);
