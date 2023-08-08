using StepLang.Tokenizing;

namespace StepLang.Parsing.Expressions;

internal sealed class UnexpectedEndOfExpressionException : ParserException
{
    public UnexpectedEndOfExpressionException(Token? token) : base(token, "Unexpected end of expression")
    {
    }
}