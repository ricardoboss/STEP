using StepLang.Tokenizing;

namespace StepLang.Interpreting;

public abstract class IncompatibleTypesException : InterpreterException
{
    protected IncompatibleTypesException(int errorCode, TokenLocation? location, string message, string helpText, Exception? inner = null) : base(errorCode, location, message, helpText, inner)
    {
    }
}