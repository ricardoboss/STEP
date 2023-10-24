using StepLang.Expressions.Results;
using StepLang.Parsing;

namespace StepLang.Expressions;

public class LogicalAndExpression : BinaryExpression
{
    public LogicalAndExpression(Expression leftExpression, Expression rightExpression) : base(leftExpression, rightExpression, BinaryExpressionOperator.LogicalAnd)
    {
    }

    protected override ExpressionResult Combine(ExpressionResult left, ExpressionResult right)
    {
        if (left is not BoolResult aBool || right is not BoolResult bBool)
            throw new IncompatibleExpressionOperandsException(left, right, "logical and");

        return new BoolResult(aBool.Value && bBool.Value);
    }
}