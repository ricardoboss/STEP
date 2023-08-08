using StepLang.Tokenizing;

namespace StepLang.Interpreting;

public class InvalidResultTypeException : InterpreterException
{
    public InvalidResultTypeException(string expected, string got) : base((TokenLocation?)null, $"Invalid result type: expected {expected}, got {got}", "An expression evaluated to an unexpected type. Check what types of expression results are allowed in the current context.")
    {
    }
}