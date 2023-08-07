using System.Diagnostics.CodeAnalysis;
using StepLang.Tokenizing;

namespace StepLang.Parsing;

[SuppressMessage("Design", "CA1032:Implement standard exception constructors")]
public abstract class ParserException : Exception
{
    public Token? Token { get; }
    public TokenLocation? Location { get; }

    protected ParserException(Token? token, string message) : base(message)
    {
        Token = token;
    }

    protected ParserException(TokenLocation? location, string message) : base(message)
    {
        Location = location;
    }
}