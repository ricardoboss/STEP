using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Expressions;

internal sealed class InvalidArgumentValueException : InterpreterException
{
    public InvalidArgumentValueException(TokenLocation location, string message) : base(7, location, message, "Make sure you're passing a value supported by the function.")
    {
    }
}