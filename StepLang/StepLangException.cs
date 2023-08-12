using StepLang.Tokenizing;

namespace StepLang;

public abstract class StepLangException : Exception
{
    public string HelpText { get; }

    public TokenLocation? Location { get; }

    protected StepLangException(TokenLocation? location, string message, string helpText, Exception? innerException) : base(message, innerException)
    {
        HelpText = helpText;
        Location = location;
    }
}