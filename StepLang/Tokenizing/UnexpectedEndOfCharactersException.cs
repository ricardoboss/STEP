using System.Diagnostics.CodeAnalysis;

namespace StepLang.Tokenizing;

[SuppressMessage("Design", "CA1032:Implement standard exception constructors")]
public class UnexpectedEndOfCharactersException : TokenizerException
{
    public UnexpectedEndOfCharactersException() : base(null, "Unexpected end of character input")
    {
    }

    public UnexpectedEndOfCharactersException(string message) : base(null, message)
    {
    }
}