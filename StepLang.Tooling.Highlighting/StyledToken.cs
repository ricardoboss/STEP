using StepLang.Tokenizing;

namespace StepLang.Tooling.Highlighting;

public record StyledToken(TokenType Type, string Text, Style Style);
