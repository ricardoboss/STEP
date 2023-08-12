namespace StepLang.Tokenizing;

public class UnexpectedEndOfCharactersException : TokenizerException
{
    public UnexpectedEndOfCharactersException(TokenLocation? location) : base(location, "Unexpected end of character input", "The current token was not complete. Check your spelling, parentheses, and other punctuation.")
    {
    }
}