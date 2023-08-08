using StepLang.Tokenizing;

namespace StepLang.Parsing.Statements;

internal sealed class MissingConditionExpressionException : ParserException
{
    public MissingConditionExpressionException(Token token, Exception inner) : base(token, "A condition was expected, but none was found", inner)
    {
    }
}