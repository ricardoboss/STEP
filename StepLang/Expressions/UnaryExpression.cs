using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Expressions;

public class UnaryExpression : Expression
{
    public static Expression Not(Expression expression)
    {
        return new UnaryExpression("!", expression, result =>
        {
            var value = result.ExpectBool().Value;

            return new BoolResult(!value);
        });
    }

    public static Expression Negate(Expression expression)
    {
        return new UnaryExpression("-", expression, result =>
        {
            var value = result.ExpectNumber().Value;

            return new NumberResult(-value);
        });
    }

    private readonly string debugName;
    private readonly Expression expression;
    private readonly Func<ExpressionResult, ExpressionResult> transform;

    public UnaryExpression(string debugName, Expression expression, Func<ExpressionResult, ExpressionResult> transform)
    {
        this.debugName = debugName;
        this.expression = expression;
        this.transform = transform;
    }

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        var result = await expression.EvaluateAsync(interpreter, cancellationToken);

        return transform.Invoke(result);
    }

    protected override string DebugDisplay() => $"({debugName} ({expression})";
}