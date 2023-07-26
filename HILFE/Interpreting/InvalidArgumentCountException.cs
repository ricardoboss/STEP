using System.Diagnostics.CodeAnalysis;

namespace HILFE.Interpreting;

[SuppressMessage("Design", "CA1032:Implement standard exception constructors")]
public class InvalidArgumentCountException : InterpreterException
{
    public InvalidArgumentCountException(int required, int got, int? allowed = null) : base($"Invalid number of arguments, expected {required}{(allowed is not null ? $"-{allowed}" : string.Empty)}, got {got}")
    {
    }
}