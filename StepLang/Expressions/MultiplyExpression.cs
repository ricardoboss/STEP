using StepLang.Expressions.Results;
using StepLang.Parsing;

namespace StepLang.Expressions;

public class MultiplyExpression : BinaryExpression
{
    public MultiplyExpression(Expression leftExpression, Expression rightExpression) : base(leftExpression, rightExpression, BinaryExpressionOperator.Multiply)
    {
    }

    protected override ExpressionResult Combine(ExpressionResult left, ExpressionResult right)
    {
        if (left is not NumberResult aNumber || right is not NumberResult bNumber)
            throw new IncompatibleExpressionOperandsException(left, right, "multiply");

        return new NumberResult(aNumber.Value * bNumber.Value);
    }
}