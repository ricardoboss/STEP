using System.Diagnostics.CodeAnalysis;
using StepLang.Tokenizing;

namespace StepLang.Parsing;

[SuppressMessage("Design", "CA1032:Implement standard exception constructors")]
public abstract class ParserException : Exception
{
    public Token? Token { get; }

    protected ParserException(string message) : this(null, message)
    {
    }

    protected ParserException(Token? token, string message) : base($"{token?.Location?.ToString() ?? "<unknown>"}: {message}")
    {
        Token = token;
    }
}