using StepLang.Tokenizing;

namespace StepLang.Tooling.Highlighting;

public record StyledToken(TokenLocation Location, TokenType Type, string Text, Style Style);
