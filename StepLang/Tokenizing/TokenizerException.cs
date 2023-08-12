namespace StepLang.Tokenizing;

public abstract class TokenizerException : StepLangException
{
    protected TokenizerException(TokenLocation? location, string message, string helpText, Exception? inner = null) : base(location, message, helpText, inner)
    {
    }
}