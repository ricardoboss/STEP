using StepLang.Expressions.Results;
using StepLang.Tokenizing;

namespace StepLang.Parsing;

public record AddExpressionNode(TokenLocation OperatorLocation, ExpressionNode Left, ExpressionNode Right) : BinaryExpressionNode(OperatorLocation, Left, Right, BinaryExpressionOperator.Add)
{
    public BinaryExpressionOperator Operator => BinaryExpressionOperator.Add;

    public override ExpressionResult EvaluateUsing(IExpressionEvaluator evaluator)
    {
        return evaluator.Evaluate(this);
    }
}