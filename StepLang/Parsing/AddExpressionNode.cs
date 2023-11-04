using StepLang.Expressions.Results;

namespace StepLang.Parsing;

public record AddExpressionNode(ExpressionNode Left, ExpressionNode Right): ExpressionNode, IBinaryExpressionNode
{
    public BinaryExpressionOperator Op => BinaryExpressionOperator.Add;

    public override ExpressionResult EvaluateUsing(IExpressionEvaluator evaluator)
    {
        return evaluator.Evaluate(this);
    }
}