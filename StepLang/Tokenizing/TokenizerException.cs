namespace StepLang.Tokenizing;

public abstract class TokenizerException : StepLangException
{
    protected TokenizerException(int errorCode, TokenLocation? location, string message, string helpText, Exception? inner = null) : base($"TOK{errorCode:000}", location, message, helpText, inner)
    {
    }
}