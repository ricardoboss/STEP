using StepLang.Expressions.Results;

namespace StepLang.Parsing;

public record BitwiseOrExpressionNode(ExpressionNode Left, ExpressionNode Right) : ExpressionNode, IBinaryExpressionNode
{
    public BinaryExpressionOperator Operator => BinaryExpressionOperator.BitwiseOr;

    public override ExpressionResult EvaluateUsing(IExpressionEvaluator evaluator)
    {
        return evaluator.Evaluate(this);
    }
}