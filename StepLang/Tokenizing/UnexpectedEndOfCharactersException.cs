namespace StepLang.Tokenizing;

internal sealed class UnexpectedEndOfCharactersException : TokenizerException
{
    public UnexpectedEndOfCharactersException(TokenLocation? location) : base(1, location, "Unexpected end of character input", "The current token was not complete. Check your spelling, parentheses, and other punctuation.")
    {
    }
}