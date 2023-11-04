using StepLang.Expressions.Results;

namespace StepLang.Parsing;

public record BitwiseRotateLeftExpressionNode(ExpressionNode Left, ExpressionNode Right) : ExpressionNode, IBinaryExpressionNode
{
    public BinaryExpressionOperator Op => BinaryExpressionOperator.BitwiseRotateLeft;

    public override ExpressionResult EvaluateUsing(IExpressionEvaluator evaluator)
    {
        return evaluator.Evaluate(this);
    }
}