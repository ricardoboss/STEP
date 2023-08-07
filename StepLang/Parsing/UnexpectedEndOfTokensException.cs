using System.Diagnostics.CodeAnalysis;
using StepLang.Tokenizing;

namespace StepLang.Parsing;

[SuppressMessage("Design", "CA1032:Implement standard exception constructors")]
public class UnexpectedEndOfTokensException : ParserException
{
    public UnexpectedEndOfTokensException(TokenLocation? location) : base(location, "Expected a statement")
    {
    }

    public UnexpectedEndOfTokensException(TokenLocation? location, string message) : base(location, message)
    {
    }
}