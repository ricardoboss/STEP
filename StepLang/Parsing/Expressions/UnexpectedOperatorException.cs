using StepLang.Tokenizing;

namespace StepLang.Parsing.Expressions;

internal sealed class UnexpectedOperatorException : ParserException
{
    public UnexpectedOperatorException(Token token, string message) : base(token, message, "An operator was found where it was not expected")
    {
    }
}