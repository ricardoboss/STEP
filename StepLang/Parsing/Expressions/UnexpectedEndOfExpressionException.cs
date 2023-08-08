using StepLang.Tokenizing;

namespace StepLang.Parsing.Expressions;

internal sealed class UnexpectedEndOfExpressionException : ParserException
{
    public UnexpectedEndOfExpressionException(Token? token) : base(token, "Unexpected end of expression", "Many expressions consist of an operation with two operands. Make sure you have provided both operands and the operator. Otherwise make sure you are using the operator in the right way.")
    {
    }
}