namespace StepLang.Interpreting;

public class IncompatibleTypesException : InterpreterException
{
    public IncompatibleTypesException(string aType, string bType, string action) : base($"Incompatible types: cannot {action} values with types {aType} and {bType}")
    {
    }
}