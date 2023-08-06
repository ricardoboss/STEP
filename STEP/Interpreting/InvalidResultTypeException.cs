using System.Diagnostics.CodeAnalysis;

namespace STEP.Interpreting;

[SuppressMessage("Design", "CA1032:Implement standard exception constructors")]
public class InvalidResultTypeException : InterpreterException
{
    public InvalidResultTypeException(string expected, string got) : base($"Invalid result type: expected {expected}, got {got}")
    {
    }
}