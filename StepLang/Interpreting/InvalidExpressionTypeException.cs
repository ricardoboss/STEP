using StepLang.Tokenizing;

namespace StepLang.Interpreting;

public class InvalidExpressionTypeException : InterpreterException
{
    public InvalidExpressionTypeException(string expected, string got) : base(4, (TokenLocation?)null, $"Invalid expression type, expected {expected}, got {got}", "Only pass the expected type of expression to this function.")
    {
    }
}