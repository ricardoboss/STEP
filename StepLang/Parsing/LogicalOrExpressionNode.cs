using StepLang.Expressions.Results;

namespace StepLang.Parsing;

public record LogicalOrExpressionNode(ExpressionNode Left, ExpressionNode Right) : ExpressionNode, IBinaryExpressionNode
{
    public BinaryExpressionOperator Op => BinaryExpressionOperator.LogicalOr;

    public override ExpressionResult EvaluateUsing(IExpressionEvaluator evaluator)
    {
        return evaluator.Evaluate(this);
    }
}