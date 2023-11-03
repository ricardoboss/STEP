using StepLang.Expressions.Results;

namespace StepLang.Parsing;

public sealed record PrimaryExpressionNode(ExpressionNode Expression) : ExpressionNode
{
    public override ExpressionResult EvaluateUsing(IExpressionEvaluator evaluator)
    {
        return evaluator.Evaluate(this);
    }
}