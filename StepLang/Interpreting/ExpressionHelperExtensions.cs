using StepLang.Expressions.Results;
using StepLang.Parsing;

namespace StepLang.Interpreting;

public static class ExpressionHelperExtensions
{
    public static IEnumerable<ExpressionResult> EvaluateUsing(this IEnumerable<ExpressionNode> nodes, Interpreter interpreter)
    {
        return nodes.Select(node => node.EvaluateUsing(interpreter));
    }
}