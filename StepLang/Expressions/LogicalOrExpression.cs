using StepLang.Expressions.Results;
using StepLang.Parsing;

namespace StepLang.Expressions;

public class LogicalOrExpression : BinaryExpression
{
    public LogicalOrExpression(Expression leftExpression, Expression rightExpression) : base(leftExpression, rightExpression, BinaryExpressionOperator.LogicalOr)
    {
    }

    protected override ExpressionResult Combine(ExpressionResult left, ExpressionResult right)
    {
        if (left is not BoolResult aBool || right is not BoolResult bBool)
            throw new IncompatibleExpressionOperandsException(left, right, "logical or");

        return new BoolResult(aBool.Value || bBool.Value);
    }
}