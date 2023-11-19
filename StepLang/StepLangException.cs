using StepLang.Tokenizing;

namespace StepLang;

public abstract class StepLangException : Exception
{
    public string HelpText {
        get; }

    public TokenLocation? Location { get; }

    public string ErrorCode { get; }

    protected StepLangException(string errorCode, TokenLocation? location, string message, string helpText, Exception? innerException) : base(message, innerException)
    {
        HelpText = helpText;
        Location = location;
        ErrorCode = errorCode;

        // ReSharper disable once VirtualMemberCallInConstructor
        HelpLink = $"https://github.com/ricardoboss/STEP/wiki/{ErrorCode}";
    }
}