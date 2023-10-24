using StepLang.Expressions.Results;
using StepLang.Parsing;

namespace StepLang.Expressions;

public class CoalesceExpression : BinaryExpression
{
    public CoalesceExpression(Expression leftExpression, Expression rightExpression) : base(leftExpression, rightExpression, BinaryExpressionOperator.Coalesce)
    {
    }

    protected override ExpressionResult Combine(ExpressionResult left, ExpressionResult right)
    {
        return left is NullResult ? right : left;
    }
}