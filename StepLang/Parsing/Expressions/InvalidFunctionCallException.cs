using System.Diagnostics.CodeAnalysis;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Expressions;

[SuppressMessage("Design", "CA1032:Implement standard exception constructors")]
public class InvalidFunctionCallException : InterpreterException
{
    public InvalidFunctionCallException(TokenLocation? callLocation, Exception inner) : base(callLocation, $"Invalid function call: {inner.Message}", inner)
    {
    }
}