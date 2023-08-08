using StepLang.Tokenizing;

namespace StepLang.Parsing.Statements;

internal sealed class MissingValueExpressionException : ParserException
{
    public MissingValueExpressionException(Token token, Exception inner) : base(token, "A value was expected, but none was found", inner)
    {
    }
}