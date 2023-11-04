using StepLang.Expressions.Results;
using StepLang.Tokenizing;

namespace StepLang.Parsing;

public record SubtractExpressionNode(TokenLocation OperatorLocation, ExpressionNode Left, ExpressionNode Right) : BinaryExpressionNode(OperatorLocation, Left, Right, BinaryExpressionOperator.Subtract)
{
    public BinaryExpressionOperator Operator => BinaryExpressionOperator.Subtract;

    public override ExpressionResult EvaluateUsing(IExpressionEvaluator evaluator)
    {
        return evaluator.Evaluate(this);
    }
}