using StepLang.Expressions.Results;

namespace StepLang.Expressions;

public class NegateExpression : UnaryExpression
{
    public NegateExpression(Expression expression) : base("-", expression)
    {
    }

    protected override ExpressionResult Transform(ExpressionResult result)
    {
        var value = result.ExpectNumber().Value;

        return new NumberResult(-value);
    }
}