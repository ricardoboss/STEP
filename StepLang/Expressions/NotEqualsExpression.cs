using StepLang.Expressions.Results;
using StepLang.Parsing;

namespace StepLang.Expressions;

public class NotEqualsExpression : BinaryExpression
{
    public NotEqualsExpression(Expression leftExpression, Expression rightExpression) : base(leftExpression, rightExpression, BinaryExpressionOperator.NotEqual)
    {
    }

    protected override ExpressionResult Combine(ExpressionResult left, ExpressionResult right)
    {
        if (left is VoidResult || right is VoidResult)
            throw new IncompatibleExpressionOperandsException(left, right, "compare (not equals)");

        if (left is NullResult && right is NullResult)
            return new BoolResult(false);

        if (left is NullResult || right is NullResult)
            return new BoolResult(true);

        return new BoolResult(left switch
        {
            StringResult aString when right is StringResult bString => !string.Equals(aString.Value, bString.Value, StringComparison.Ordinal),
            NumberResult aNumber when right is NumberResult bNumber => Math.Abs(aNumber.Value - bNumber.Value) >= double.Epsilon,
            BoolResult aBool when right is BoolResult bBool => aBool.Value != bBool.Value,
            _ => true,
        });
    }
}