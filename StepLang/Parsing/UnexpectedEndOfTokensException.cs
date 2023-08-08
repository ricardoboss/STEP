using StepLang.Tokenizing;

namespace StepLang.Parsing;

public class UnexpectedEndOfTokensException : ParserException
{
    public UnexpectedEndOfTokensException(TokenLocation? location) : base(location, "Expected a statement")
    {
    }

    public UnexpectedEndOfTokensException(TokenLocation? location, string message) : base(location, message)
    {
    }
}