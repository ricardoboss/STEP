namespace StepLang.Interpreting;

public class InvalidExpressionTypeException : InterpreterException
{
    public InvalidExpressionTypeException(string expected, string got) : base($"Invalid expression type, expected {expected}, got {got}")
    {
    }
}