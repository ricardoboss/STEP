using StepLang.Expressions.Results;

namespace StepLang.Parsing;

public record BitwiseXorExpressionNode(ExpressionNode Left, ExpressionNode Right) : ExpressionNode, IBinaryExpressionNode
{
    public BinaryExpressionOperator Operator => BinaryExpressionOperator.BitwiseXor;

    public override ExpressionResult EvaluateUsing(IExpressionEvaluator evaluator)
    {
        return evaluator.Evaluate(this);
    }
}