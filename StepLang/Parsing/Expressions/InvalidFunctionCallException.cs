using StepLang.Tokenizing;

namespace StepLang.Parsing.Expressions;

public class InvalidFunctionCallException : ParserException
{
    public InvalidFunctionCallException(TokenLocation? callLocation, StepLangException inner) : base(callLocation, $"Invalid function call: {inner.Message}", inner.HelpText ?? "Check the function documentation on how to call this function.", inner)
    {
    }
}