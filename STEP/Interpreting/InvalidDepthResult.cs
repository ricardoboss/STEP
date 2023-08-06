using System.Diagnostics.CodeAnalysis;
using STEP.Parsing.Expressions;

namespace STEP.Interpreting;

[SuppressMessage("Design", "CA1032:Implement standard exception constructors")]
public class InvalidDepthResult : InterpreterException
{
    public InvalidDepthResult(string keyword, ExpressionResult result) : base($"Invalid {keyword} depth: {keyword} depth must be a positive number, got {result}")
    {
    }
}