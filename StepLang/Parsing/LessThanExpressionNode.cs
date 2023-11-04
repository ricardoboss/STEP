using StepLang.Expressions.Results;
using StepLang.Tokenizing;

namespace StepLang.Parsing;

public record LessThanExpressionNode(TokenLocation OperatorLocation, ExpressionNode Left, ExpressionNode Right) : BinaryExpressionNode(OperatorLocation, Left, Right, BinaryExpressionOperator.LessThan)
{
    public BinaryExpressionOperator Operator => BinaryExpressionOperator.LessThan;

    public override ExpressionResult EvaluateUsing(IExpressionEvaluator evaluator)
    {
        return evaluator.Evaluate(this);
    }
}