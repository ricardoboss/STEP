using StepLang.Expressions.Results;

namespace StepLang.Parsing;

public record LogicalAndExpressionNode(ExpressionNode Left, ExpressionNode Right) : ExpressionNode, IBinaryExpressionNode
{
    public BinaryExpressionOperator Op => BinaryExpressionOperator.LogicalAnd;

    public override ExpressionResult EvaluateUsing(IExpressionEvaluator evaluator)
    {
        return evaluator.Evaluate(this);
    }
}