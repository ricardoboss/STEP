using System.Diagnostics.CodeAnalysis;

namespace HILFE.Interpreting;

[SuppressMessage("Design", "CA1032:Implement standard exception constructors")]
public class InterpreterException : Exception
{
    public InterpreterException(string message) : base(message)
    {
    }
}