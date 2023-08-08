namespace StepLang.Tokenizing;

internal sealed class InvalidIdentifierException : TokenizerException
{
    public InvalidIdentifierException(TokenLocation? location, string value) : base(location, $"Invalid identifier '{value}'")
    {
    }
}