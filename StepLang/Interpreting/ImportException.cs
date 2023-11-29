using StepLang.Tokenizing;

namespace StepLang.Interpreting;

public abstract class ImportException(int errorCode, TokenLocation location, string message, string helpText)
    : InterpreterException($"IMP{errorCode:000}", location, message, helpText);
