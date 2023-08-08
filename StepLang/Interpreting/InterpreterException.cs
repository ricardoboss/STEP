using StepLang.Parsing.Statements;
using StepLang.Tokenizing;

namespace StepLang.Interpreting;

public abstract class InterpreterException : Exception
{
    public Statement? Statement { get; }
    public Token? Token { get; }
    public TokenLocation? Location { get; }

    protected InterpreterException(string message) : base(message)
    {
    }

    protected InterpreterException(Statement? statement, string message, Exception? inner = null) : base(message, inner)
    {
        Statement = statement;
    }

    protected InterpreterException(Token? token, string message, Exception? inner = null) : base(message, inner)
    {
        Token = token;
    }

    protected InterpreterException(TokenLocation? location, string message, Exception? inner = null) : base(message, inner)
    {
        Location = location;
    }
}