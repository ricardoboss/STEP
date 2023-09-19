using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;

namespace StepLang.Expressions;

public abstract class Expression
{
    public abstract Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, CancellationToken cancellationToken = default);

    public async Task<T> EvaluateAsync<T>(Interpreter interpreter, Func<ExpressionResult, T> selector, CancellationToken cancellationToken = default)
    {
        var result = await EvaluateAsync(interpreter, cancellationToken);

        return selector(result);
    }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        var debugDisplay = DebugDisplay();
        if (debugDisplay.Length > 0)
            debugDisplay = $": {debugDisplay}";

        return $"<{GetType().Name}{debugDisplay}>";
    }

    [ExcludeFromCodeCoverage]
    protected virtual string DebugDisplay() => "";
}