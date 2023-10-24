using StepLang.Expressions.Results;
using StepLang.Parsing;

namespace StepLang.Expressions;

public class LessThanExpression : BinaryExpression
{
    public LessThanExpression(Expression leftExpression, Expression rightExpression) : base(leftExpression, rightExpression, BinaryExpressionOperator.LessThan)
    {
    }

    protected override ExpressionResult Combine(ExpressionResult left, ExpressionResult right)
    {
        if (left is not NumberResult aNumber || right is not NumberResult bNumber)
            throw new IncompatibleExpressionOperandsException(left, right, "compare (less than)");

        return new BoolResult(aNumber.Value < bNumber.Value);
    }
}