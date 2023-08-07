using System.Diagnostics.CodeAnalysis;

namespace StepLang.Parsing;

[SuppressMessage("Design", "CA1032:Implement standard exception constructors")]
public class UnexpectedEndOfTokensException : ParserException
{
    public UnexpectedEndOfTokensException() : base(null, "Unexpected end of tokens")
    {
    }

    public UnexpectedEndOfTokensException(string message) : base(null, message)
    {
    }
}