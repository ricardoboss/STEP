using StepLang.Tokenizing;

namespace StepLang.Parsing;

internal sealed class MissingExpressionException(Token? token) : ParserException(3, token,
	"A value was expected, but none was found",
	$"Make sure your expression comes directly after the {TokenType.EqualsSymbol.ToDisplay()} in an assignment or declaration.");
