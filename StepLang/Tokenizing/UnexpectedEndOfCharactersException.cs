using System.Diagnostics.CodeAnalysis;

namespace StepLang.Tokenizing;

[SuppressMessage("Design", "CA1032:Implement standard exception constructors")]
public class UnexpectedEndOfCharactersException : TokenizerException
{
    public UnexpectedEndOfCharactersException(TokenLocation? location) : base(location, "Unexpected end of character input")
    {
    }
}