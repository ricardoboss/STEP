using StepLang.Tokenizing;

namespace StepLang.Parsing.Expressions;

internal sealed class InvalidExpressionException : ParserException
{
    public InvalidExpressionException(Token? token, string message) : base(8, token, message, "Check the syntax of your expression. Make sure all operators are used correctly and all operands are present. Try to read the expression as if you needed to calculate the result by hand.")
    {
    }
}