using StepLang.Expressions.Results;

namespace StepLang.Parsing;

public record DivideExpressionNode(ExpressionNode Left, ExpressionNode Right) : ExpressionNode
{
    public override ExpressionResult EvaluateUsing(IExpressionEvaluator evaluator)
    {
        return evaluator.Evaluate(this);
    }
}