using StepLang.Tokenizing;

namespace StepLang.Interpreting;

public abstract class InterpreterException : StepLangException
{
    protected InterpreterException(int errorCode, Token token, string message, string helpText, Exception? inner = null) : this(errorCode, token.Location, message, helpText, inner)
    {
    }

    protected InterpreterException(int errorCode, TokenLocation location, string message, string helpText, Exception? inner = null) : this($"INT{errorCode:000}", location, message, helpText, inner)
    {
    }

    protected InterpreterException(string errorCode, TokenLocation location, string message, string helpText, Exception? inner = null) : base(errorCode, location, message, helpText, inner)
    {
    }
}