using StepLang.Expressions.Results;
using StepLang.Tokenizing;

namespace StepLang.Parsing;

public record GreaterThanOrEqualExpressionNode(TokenLocation OperatorLocation, ExpressionNode Left, ExpressionNode Right) : BinaryExpressionNode(OperatorLocation, Left, Right, BinaryExpressionOperator.GreaterThanOrEqual)
{
    public override ExpressionResult EvaluateUsing(IExpressionEvaluator evaluator)
    {
        return evaluator.Evaluate(this);
    }
}