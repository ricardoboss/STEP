using StepLang.Tokenizing;

namespace StepLang.Interpreting;

public class InvalidExpressionTypeException : InterpreterException
{
    public InvalidExpressionTypeException(TokenLocation location, string expected, string got) : base(4, location, $"Invalid expression type, expected {expected}, got {got}", "Only pass the expected type of expression to this function.")
    {
    }
}