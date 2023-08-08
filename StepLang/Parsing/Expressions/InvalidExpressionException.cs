using StepLang.Tokenizing;

namespace StepLang.Parsing.Expressions;

internal sealed class InvalidExpressionException : ParserException
{
    public InvalidExpressionException(Token? token, string message) : base(token, message)
    {
    }
}