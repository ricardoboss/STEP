using StepLang.Expressions.Results;
using StepLang.Parsing;

namespace StepLang.Expressions;

public class DivideExpression : BinaryExpression
{
    public DivideExpression(Expression leftExpression, Expression rightExpression) : base(leftExpression, rightExpression, BinaryExpressionOperator.Divide)
    {
    }

    protected override ExpressionResult Combine(ExpressionResult left, ExpressionResult right)
    {
        if (left is not NumberResult aNumber || right is not NumberResult bNumber)
            throw new IncompatibleExpressionOperandsException(left, right, "divide");

        // TODO: throw a specific exception when dividing by zero

        return new NumberResult(aNumber.Value / bNumber.Value);
    }
}