using StepLang.Expressions.Results;
using StepLang.Parsing;

namespace StepLang.Expressions;

public class ModuloExpression : BinaryExpression
{
    public ModuloExpression(Expression leftExpression, Expression rightExpression) : base(leftExpression, rightExpression, BinaryExpressionOperator.Modulo)
    {
    }

    protected override ExpressionResult Combine(ExpressionResult left, ExpressionResult right)
    {
        if (left is not NumberResult aNumber || right is not NumberResult bNumber)
            throw new IncompatibleExpressionOperandsException(left, right, "modulo");

        return new NumberResult(aNumber.Value % bNumber.Value);
    }
}