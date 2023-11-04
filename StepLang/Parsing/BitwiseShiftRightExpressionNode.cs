using StepLang.Expressions.Results;

namespace StepLang.Parsing;

public record BitwiseShiftRightExpressionNode(ExpressionNode Left, ExpressionNode Right) : ExpressionNode, IBinaryExpressionNode
{
    public BinaryExpressionOperator Operator => BinaryExpressionOperator.BitwiseShiftRight;

    public override ExpressionResult EvaluateUsing(IExpressionEvaluator evaluator)
    {
        return evaluator.Evaluate(this);
    }
}