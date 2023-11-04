using StepLang.Expressions.Results;
using StepLang.Tokenizing;

namespace StepLang.Parsing;

public record EqualsExpressionNode(TokenLocation OperatorLocation, ExpressionNode Left, ExpressionNode Right) : BinaryExpressionNode(OperatorLocation, Left, Right, BinaryExpressionOperator.Equal)
{
    public BinaryExpressionOperator Operator => BinaryExpressionOperator.Equal;

    public override ExpressionResult EvaluateUsing(IExpressionEvaluator evaluator)
    {
        return evaluator.Evaluate(this);
    }
}