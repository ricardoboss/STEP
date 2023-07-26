using System.Diagnostics.CodeAnalysis;

namespace HILFE.Interpreting;

[SuppressMessage("Design", "CA1032:Implement standard exception constructors")]
public class UndefinedIdentifierException : InterpreterException
{
    public UndefinedIdentifierException(string name) : base("Undefined variable: " + name)
    {
    }
}