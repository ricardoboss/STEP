using StepLang.Expressions.Results;
using StepLang.Parsing;

namespace StepLang.Expressions;

public class SubtractExpression : BinaryExpression
{
    public SubtractExpression(Expression leftExpression, Expression rightExpression) : base(leftExpression, rightExpression, BinaryExpressionOperator.Subtract)
    {
    }

    protected override ExpressionResult Combine(ExpressionResult left, ExpressionResult right)
    {
        if (left is not NumberResult aNumber || right is not NumberResult bNumber)
            throw new IncompatibleExpressionOperandsException(left, right, "subtract");

        return new NumberResult(aNumber.Value - bNumber.Value);
    }
}