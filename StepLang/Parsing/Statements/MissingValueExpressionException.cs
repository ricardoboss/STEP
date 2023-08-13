using StepLang.Tokenizing;

namespace StepLang.Parsing.Statements;

internal sealed class MissingValueExpressionException : ParserException
{
    public MissingValueExpressionException(Token token, Exception inner) : base(3, token, "A value was expected, but none was found", $"Make sure your expression comes directly after the {TokenType.EqualsSymbol.ToDisplay()} in an assignment or declaration.", inner)
    {
    }
}