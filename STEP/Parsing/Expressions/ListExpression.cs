using STEP.Interpreting;

namespace STEP.Parsing.Expressions;

internal class ListExpression : Expression
{
    private readonly IReadOnlyList<Expression> expressions;

    public ListExpression(IReadOnlyList<Expression> expressions)
    {
        this.expressions = expressions;
    }

    /// <inheritdoc />
    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        var values = await expressions
            .EvaluateAsync(interpreter, cancellationToken)
            .ToArrayAsync(cancellationToken);

        return ExpressionResult.List(values);
    }

    /// <inheritdoc />
    protected override string DebugDisplay() => $"[{string.Join(", ", expressions.Select(e => e.ToString()))}]";
}