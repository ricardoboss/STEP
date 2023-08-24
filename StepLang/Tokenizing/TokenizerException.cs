namespace StepLang.Tokenizing;

public abstract class TokenizerException : StepLangException
{
    protected TokenizerException(string message, TokenLocation? location, string helpText, Exception? inner = null) : base(location, message, helpText, inner)
    {
    }
}