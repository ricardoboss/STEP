using StepLang.Tokenizing;

namespace StepLang.Interpreting;

public class InvalidArgumentCountException : InterpreterException
{
    public InvalidArgumentCountException(int required, int got, int? allowed = null) : base((TokenLocation?)null, $"Invalid number of arguments, expected {(allowed is null ? "at least " : "")}{required}{(allowed is not null ? $"-{allowed}" : "")}, got {got}", "Check the function documentation on the required/allowed number of arguments")
    {
    }
}