using System.Diagnostics.CodeAnalysis;
using StepLang.Tokenizing;

namespace StepLang.Interpreting;

[SuppressMessage("Design", "CA1032:Implement standard exception constructors")]
public abstract class InterpreterException : Exception
{
    protected InterpreterException(string message) : base(message)
    {
    }

    protected InterpreterException(string message, Exception inner) : base(message, inner)
    {
    }

    protected InterpreterException(Token? token, string message) : base($"{token?.Location?.ToString() ?? "<unknown>"}: {message}")
    {
    }
}