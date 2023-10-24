using StepLang.Expressions.Results;
using StepLang.Parsing;

namespace StepLang.Expressions;

public class EqualsExpression : BinaryExpression
{
    public EqualsExpression(Expression leftExpression, Expression rightExpression) : base(leftExpression, rightExpression, BinaryExpressionOperator.Equal)
    {
    }

    protected override ExpressionResult Combine(ExpressionResult left, ExpressionResult right)
    {
        if (left is VoidResult || right is VoidResult)
            throw new IncompatibleExpressionOperandsException(left, right, "compare (equals)");

        if (left is NullResult && right is NullResult)
            return new BoolResult(true);

        if (left is NullResult || right is NullResult)
            return new BoolResult(false);

        return new BoolResult(left switch
        {
            StringResult aString when right is StringResult bString => string.Equals(aString.Value, bString.Value, StringComparison.Ordinal),
            NumberResult aNumber when right is NumberResult bNumber => Math.Abs(aNumber.Value - bNumber.Value) < double.Epsilon,
            BoolResult aBool when right is BoolResult bBool => aBool.Value == bBool.Value,
            _ => false,
        });
    }
}