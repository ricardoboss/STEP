namespace StepLang.Tokenizing;

public abstract class TokenizerException : Exception
{
    public TokenLocation? Location { get; }

    protected TokenizerException(TokenLocation? location, string message) : base(message)
    {
        Location = location;
    }
}