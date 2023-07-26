using System.Diagnostics.CodeAnalysis;

namespace HILFE.Interpreting;

[SuppressMessage("Design", "CA1032:Implement standard exception constructors")]
public class InvalidExpressionTypeException : InterpreterException
{
    public InvalidExpressionTypeException(string expected, string got) : base($"Invalid expression type, expected {expected}, got {got}")
    {
    }
}