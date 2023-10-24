using StepLang.Expressions.Results;

namespace StepLang.Expressions;

public class NotExpression : UnaryExpression
{
    public NotExpression(Expression expression) : base("!", expression)
    {
    }

    protected override ExpressionResult Transform(ExpressionResult result)
    {
        var value = result.ExpectBool().Value;

        return new BoolResult(!value);
    }
}