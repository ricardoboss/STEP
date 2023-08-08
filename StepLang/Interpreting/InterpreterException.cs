using StepLang.Parsing.Statements;
using StepLang.Tokenizing;

namespace StepLang.Interpreting;

public abstract class InterpreterException : StepLangException
{
    protected InterpreterException(Statement? statement, string message, string helpText, Exception? inner = null) : this(statement?.Location, message, helpText, inner)
    {
    }

    protected InterpreterException(Token? token, string message, string helpText, Exception? inner = null) : this(token?.Location, message, helpText, inner)
    {
    }

    protected InterpreterException(TokenLocation? location, string message, string helpText, Exception? inner = null) : base(location, message, helpText, inner)
    {
    }
}