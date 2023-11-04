using StepLang.Expressions.Results;

namespace StepLang.Parsing;

public record AddExpressionNode(ExpressionNode Left, ExpressionNode Right): ExpressionNode, IBinaryExpressionNode
{
    public BinaryExpressionOperator Operator => BinaryExpressionOperator.Add;

    public override ExpressionResult EvaluateUsing(IExpressionEvaluator evaluator)
    {
        return evaluator.Evaluate(this);
    }
}