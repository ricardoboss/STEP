using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Expressions;

public abstract class UnaryExpression : Expression
{
    private readonly string unaryExpressionSymbol;
    private readonly Expression expression;

    protected UnaryExpression(string unaryExpressionSymbol, Expression expression)
    {
        this.unaryExpressionSymbol = unaryExpressionSymbol;
        this.expression = expression;
    }

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter,
        CancellationToken cancellationToken = default)
    {
        var result = await expression.EvaluateAsync(interpreter, cancellationToken);

        return Transform(result);
    }

    protected abstract ExpressionResult Transform(ExpressionResult result);

    protected override string DebugDisplay() => $"({unaryExpressionSymbol} ({expression})";
}