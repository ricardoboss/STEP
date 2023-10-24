using StepLang.Expressions.Results;
using StepLang.Parsing;

namespace StepLang.Expressions;

public class PowerExpression : BinaryExpression
{
    public PowerExpression(Expression leftExpression, Expression rightExpression) : base(leftExpression, rightExpression, BinaryExpressionOperator.Power)
    {
    }

    protected override ExpressionResult Combine(ExpressionResult left, ExpressionResult right)
    {
        if (left is not NumberResult aNumber || right is not NumberResult bNumber)
            throw new IncompatibleExpressionOperandsException(left, right, "power");

        return new NumberResult(Math.Pow(aNumber.Value, bNumber.Value));
    }
}