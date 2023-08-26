using StepLang.Tokenizing;

namespace StepLang.Parsing.Expressions;

public class InvalidFunctionCallException : ParserException
{
    public InvalidFunctionCallException(TokenLocation? callLocation, StepLangException inner) : base(inner.ErrorCode, callLocation, $"Invalid function call: {inner.Message}", inner.HelpText, inner)
    {
    }
}