using System.Diagnostics.CodeAnalysis;

namespace StepLang.Interpreting;

[SuppressMessage("Design", "CA1032:Implement standard exception constructors")]
public class IncompatibleTypesException : InterpreterException
{
    public IncompatibleTypesException(string aType, string bType, string action) : base($"Incompatible types: cannot {action} values with types {aType} and {bType}")
    {
    }
}