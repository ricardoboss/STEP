using System.Diagnostics.CodeAnalysis;
using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Expressions;

public abstract class FunctionDefinition
{
    public virtual IEnumerable<(ResultType[] types, string identifier)> Parameters => Enumerable.Empty<(ResultType[] types, string identifier)>();

    public abstract Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default);

    protected virtual string DebugParamsString => string.Join(", ", Parameters.Select(p => $"{string.Join("|", p.types.Select(t => t.ToTypeName()))} {p.identifier}"));

    protected abstract string DebugBodyString { get; }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        var paramStr = DebugParamsString;
        var bodyStr = DebugBodyString;

        return $"<Function: ({paramStr}) => {{ {bodyStr} }}>";
    }

    public FunctionResult ToResult() => new(this);
}