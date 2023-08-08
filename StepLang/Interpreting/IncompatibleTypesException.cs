using StepLang.Tokenizing;

namespace StepLang.Interpreting;

public abstract class IncompatibleTypesException : InterpreterException
{
    protected IncompatibleTypesException(TokenLocation? location, string message, string helpText, Exception? inner = null) : base(location, message, helpText, inner)
    {
    }
}