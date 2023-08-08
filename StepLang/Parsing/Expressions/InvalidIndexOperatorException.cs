using StepLang.Interpreting;

namespace StepLang.Parsing.Expressions;

public class InvalidIndexOperatorException : InterpreterException
{
    public InvalidIndexOperatorException(string valueType) : base($"Invalid index expression: Cannot index into a value of type {valueType}")
    {
    }
}