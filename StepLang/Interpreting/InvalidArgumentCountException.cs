namespace StepLang.Interpreting;

public class InvalidArgumentCountException : InterpreterException
{
    public InvalidArgumentCountException(int required, int got, int? allowed = null) : base($"Invalid number of arguments, expected {(allowed is null ? "at least " : "")}{required}{(allowed is not null ? $"-{allowed}" : "")}, got {got}")
    {
    }
}