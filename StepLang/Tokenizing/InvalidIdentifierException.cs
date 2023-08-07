using System.Diagnostics.CodeAnalysis;

namespace StepLang.Tokenizing;

[SuppressMessage("Design", "CA1032:Implement standard exception constructors")]
internal sealed class InvalidIdentifierException : TokenizerException
{
    public InvalidIdentifierException(TokenLocation? location, string value) : base(location, $"Invalid identifier '{value}'")
    {
    }
}