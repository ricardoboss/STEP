using HILFE.Interpreting;

namespace HILFE.Parsing.Expressions;

public abstract class FunctionDefinition
{
    public abstract Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default);

    protected virtual string DebugParamsString => string.Empty;

    protected virtual string DebugBodyString => string.Empty;
    
    /// <inheritdoc />
    public override string ToString()
    {
        var paramStr = DebugParamsString;
        var bodyStr = DebugBodyString;

        return $"<Function: ({paramStr}) => {{ {bodyStr} }}>";
    }
}