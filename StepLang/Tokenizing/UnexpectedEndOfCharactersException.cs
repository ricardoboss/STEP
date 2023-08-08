namespace StepLang.Tokenizing;

public class UnexpectedEndOfCharactersException : TokenizerException
{
    public UnexpectedEndOfCharactersException(TokenLocation? location) : base(location, "Unexpected end of character input")
    {
    }
}