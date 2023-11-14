using StepLang.Tokenizing;

namespace StepLang.Interpreting;

public class ImportException : InterpreterException
{
    public ImportException(int errorCode, TokenLocation location, string message, string helpText) : base($"IMP{errorCode:000}", location, message, helpText)
    {
    }
}
