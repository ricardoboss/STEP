using StepLang.Expressions.Results;

namespace StepLang.Parsing;

public record NegateExpressionNode(ExpressionNode Expression) : ExpressionNode, IUnaryExpressionNode
{
    public override ExpressionResult EvaluateUsing(IExpressionEvaluator evaluator)
    {
        return evaluator.Evaluate(this);
    }
}