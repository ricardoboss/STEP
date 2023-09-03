using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework.IO;

public class PrintFunction : NativeFunction
{
    public const string Identifier = "print";

    /// <inheritdoc />
    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        if (interpreter.StdOut is not { } stdOut)
            return VoidResult.Instance;

        var stringArgs = await arguments
            .EvaluateAsync(interpreter, cancellationToken)
            .Select(RenderValue)
            .ToListAsync(cancellationToken);

        await Print(stdOut, string.Join("", stringArgs), cancellationToken);

        return VoidResult.Instance;
    }

    private static string RenderValue(ExpressionResult result)
    {
        switch (result)
        {
            case StringResult { Value: var stringValue }:
                return stringValue;
            case NumberResult { Value: var numberValue }:
                return numberValue.ToString(CultureInfo.InvariantCulture);
            case BoolResult { Value: var boolValue }:
                return boolValue.ToString();
            case NullResult:
                return "null";
            case ListResult { Value: var values }:
                var renderedValues = values.Select(v =>
                {
                    string renderedValue;
                    if (v is StringResult stringValue)
                        renderedValue = $"\"{stringValue.Value}\"";
                    else
                        renderedValue = RenderValue(v);

                    return renderedValue;
                });
                return $"[{string.Join(", ", renderedValues)}]";
            case MapResult { Value: var pairs }:
                var renderedPairs = pairs.Select(pair =>
                {
                    string renderedValue;
                    if (pair.Value is StringResult stringValue)
                        renderedValue = $"\"{stringValue.Value}\"";
                    else
                        renderedValue = RenderValue(pair.Value);

                    return $"\"{pair.Key}\": {renderedValue}";
                });
                var mapContents = string.Join(", ", renderedPairs);
                return $"{{{mapContents}}}";
            default:
                return $"[{result.ResultType}]";
        }
    }

    protected virtual async Task Print(TextWriter output, string value, CancellationToken cancellationToken = default)
        => await output.WriteAsync(value.AsMemory(), cancellationToken);

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    protected override string DebugParamsString => "any ...args";
}