using StepLang.Parsing.Expressions;

namespace StepLang.Interpreting;

public class InvalidDepthResult : InterpreterException
{
    public InvalidDepthResult(string keyword, ExpressionResult result) : base($"Invalid {keyword} depth: {keyword} depth must be a positive number, got {result}")
    {
    }
}