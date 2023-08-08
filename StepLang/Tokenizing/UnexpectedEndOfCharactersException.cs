namespace StepLang.Tokenizing;

public class UnexpectedEndOfCharactersException : TokenizerException
{
    public UnexpectedEndOfCharactersException(TokenLocation? location) : base("Unexpected end of character input", location, "The current token was not complete. Check your spelling, parentheses, and other punctuation.")
    {
    }
}