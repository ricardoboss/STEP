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
            return ExpressionResult.Void;

        var stringArgs = await arguments
            .EvaluateAsync(interpreter, cancellationToken)
            .Select(RenderValue)
            .ToListAsync(cancellationToken);

        await Print(stdOut, string.Join("", stringArgs), cancellationToken);

        return ExpressionResult.Void;
    }

    private static string RenderValue(ExpressionResult result)
    {
        switch (result.ValueType)
        {
            case "string" when result.Value is string stringValue:
                return stringValue;
            case "number" when result.Value is double numberValue:
                return numberValue.ToString(CultureInfo.InvariantCulture);
            case "bool" when result.Value is bool boolValue:
                return boolValue.ToString();
            case "null" when result.Value is null:
                return "null";
            case "list" when result.Value is IList<ExpressionResult> values:
                return $"[{string.Join(", ", values.Select(RenderValue))}]";
            case "map" when result.Value is IDictionary<string, ExpressionResult> pairs:
                var renderedPairs = pairs.Select(pair =>
                {
                    var renderedValue = RenderValue(pair.Value);
                    if (pair.Value.ValueType is "string")
                        renderedValue = $"\"{renderedValue}\"";

                    return $"\"{pair.Key}\": {renderedValue}";
                });
                var mapContents = string.Join(", ", renderedPairs);
                return $"{{{mapContents}}}";
            default:
                return $"[{result.ValueType}]";
        }
    }

    protected virtual async Task Print(TextWriter output, string value, CancellationToken cancellationToken = default)
        => await output.WriteAsync(value.AsMemory(), cancellationToken);

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    protected override string DebugParamsString => "string ...args";
}