using StepLang.Tokenizing;

namespace StepLang.Interpreting;

public class InvalidArgumentCountException : InterpreterException
{
    public InvalidArgumentCountException(TokenLocation location, int required, int got, int? allowed = null) : base(2, location, $"Invalid number of arguments, expected {required}{(allowed is not null ? $"-{allowed}" : "")}, got {got}", "Check the function documentation on the required/allowed number of arguments")
    {
    }
}