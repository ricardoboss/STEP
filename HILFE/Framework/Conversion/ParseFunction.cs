using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using HILFE.Interpreting;
using HILFE.Parsing.Expressions;

namespace HILFE.Framework.Conversion;

public class ParseFunction : NativeFunction
{
    public const string Identifier = "parse";

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        if (arguments.Count != 2)
            throw new InvalidArgumentCountException(2, arguments.Count);

        var type = await arguments[0].EvaluateAsync(interpreter, cancellationToken);
        var targetType = type.ExpectString();

        var source = await arguments[1].EvaluateAsync(interpreter, cancellationToken);
        source.ThrowIfVoid();
        var (sourceType, sourceValue, _) = source;

        if (targetType == sourceType)
            return new(targetType, sourceValue);

        string stringValue = sourceValue?.ToString(CultureInfo.InvariantCulture) ?? string.Empty;
        return sourceType switch
        {
            "string" => targetType switch
            {
                "number" when double.TryParse(stringValue, NumberStyles.Any, CultureInfo.InvariantCulture, out var doubleValue) => new("number", doubleValue),
                "bool" when bool.TryParse(stringValue, out var boolValue) => new("bool", boolValue),
                "bool" when stringValue is "1" or "0" => new("bool", stringValue is "1"),
                _ => ExpressionResult.Null,
            },
            "number" => targetType switch
            {
                "string" => new("string", stringValue),
                "bool" => new("bool", sourceValue is not 0.0),
                _ => ExpressionResult.Null,
            },
            "bool" => targetType switch
            {
                "string" => new("string", stringValue),
                "number" => new("number", sourceValue is true ? 1 : 0),
                _ => ExpressionResult.Null,
            },
            _ => ExpressionResult.Null,
        };
    }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    protected override string DebugParamsString => "string type, any value";
}