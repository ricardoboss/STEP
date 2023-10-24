using StepLang.Expressions.Results;
using StepLang.Parsing;

namespace StepLang.Expressions;

public class AddExpression : BinaryExpression
{
    public AddExpression(Expression leftExpression, Expression rightExpression) : base(leftExpression, rightExpression, BinaryExpressionOperator.Add)
    {
    }

    protected override ExpressionResult Combine(ExpressionResult left, ExpressionResult right)
    {
        return left switch
        {
            NumberResult aNumber when right is NumberResult bNumber => new NumberResult(aNumber.Value + bNumber.Value),
            NumberResult aNumber when right is StringResult bString => new StringResult(aNumber.Value + bString.Value),
            StringResult aString when right is NumberResult bNumber => new StringResult(aString.Value + bNumber.Value),
            StringResult aString when right is StringResult bString => new StringResult(aString.Value + bString.Value),
            _ => throw new IncompatibleExpressionOperandsException(left, right, "add"),
        };
    }
}