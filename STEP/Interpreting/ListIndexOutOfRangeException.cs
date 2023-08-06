using System.Diagnostics.CodeAnalysis;

namespace STEP.Interpreting;

[SuppressMessage("Design", "CA1032:Implement standard exception constructors")]
public class ListIndexOutOfRangeException : InterpreterException
{
    public ListIndexOutOfRangeException(double index, int count) : base($"Index {index} is out of range for list with {count} elements")
    {
    }
}