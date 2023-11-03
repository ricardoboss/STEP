using StepLang.Expressions.Results;

namespace StepLang.Parsing;

public sealed record ListExpressionNode(IReadOnlyCollection<ExpressionNode> Expressions) : ExpressionNode
{
    public override ExpressionResult EvaluateUsing(IExpressionEvaluator evaluator)
    {
        return evaluator.Evaluate(this);
    }
}