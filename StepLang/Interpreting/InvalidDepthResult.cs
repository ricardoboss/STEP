using StepLang.Parsing.Expressions;
using StepLang.Tokenizing;

namespace StepLang.Interpreting;

public class InvalidDepthResult : InterpreterException
{
    public InvalidDepthResult(Token keywordToken, ExpressionResult result) : base(4, keywordToken.Location, $"Invalid {keywordToken.Value} depth: {keywordToken.Value} depth must be a positive number, got {result}", "Make sure to only pass positive numbers as the depth to continue or break.")
    {
    }
}