using System.Diagnostics.CodeAnalysis;
using StepLang.Parsing.Statements;
using StepLang.Tokenizing;

namespace StepLang.Interpreting;

[SuppressMessage("Design", "CA1032:Implement standard exception constructors")]
public abstract class InterpreterException : Exception
{
    public Statement? Statement { get; }
    public Token? Token { get; }

    protected InterpreterException(string message) : base(message)
    {
        Statement = null;
        Token = null;
    }

    protected InterpreterException(Statement? statement, string message, Exception inner) : base(message, inner)
    {
        Statement = statement;
        Token = null;
    }

    protected InterpreterException(Token? token, string message) : base($"{token?.Location?.ToString() ?? "<unknown>"}: {message}")
    {
        Statement = null;
        Token = token;
    }
}