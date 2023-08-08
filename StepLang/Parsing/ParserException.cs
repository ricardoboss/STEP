using StepLang.Tokenizing;

namespace StepLang.Parsing;

public abstract class ParserException : Exception
{
    public Token? Token { get; }
    public TokenLocation? Location { get; }

    protected ParserException(Token? token, string message, Exception? inner = null) : base(message, inner)
    {
        Token = token;
    }

    protected ParserException(TokenLocation? location, string message) : base(message)
    {
        Location = location;
    }
}