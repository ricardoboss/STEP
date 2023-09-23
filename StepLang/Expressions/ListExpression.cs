using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Expressions;

public sealed class ListExpression : Expression
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
            .ToListAsync(cancellationToken);

        return new ListResult(values);
    }

    /// <inheritdoc />
    protected override string DebugDisplay() => $"[{string.Join(", ", expressions.Select(e => e.ToString()))}]";
}