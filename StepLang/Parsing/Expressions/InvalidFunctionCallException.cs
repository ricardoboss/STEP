using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Expressions;

public class InvalidFunctionCallException : InterpreterException
{
    public InvalidFunctionCallException(TokenLocation? callLocation, Exception inner) : base(callLocation, $"Invalid function call: {inner.Message}", inner)
    {
    }
}