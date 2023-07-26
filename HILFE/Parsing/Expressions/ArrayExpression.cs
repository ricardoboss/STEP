using HILFE.Interpreting;

namespace HILFE.Parsing.Expressions;

internal class ArrayExpression : Expression
{
    private readonly IReadOnlyList<Expression> arrayExpressions;

    public ArrayExpression(IReadOnlyList<Expression> arrayExpressions)
    {
        this.arrayExpressions = arrayExpressions;
    }

    /// <inheritdoc />
    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        var values = await arrayExpressions
            .EvaluateAsync(interpreter, cancellationToken)
            .ToArrayAsync(cancellationToken);

        return ExpressionResult.Array(values);
    }

    /// <inheritdoc />
    protected override string DebugDisplay() => $"[{string.Join(", ", arrayExpressions.Select(e => e.ToString()))}]";
}