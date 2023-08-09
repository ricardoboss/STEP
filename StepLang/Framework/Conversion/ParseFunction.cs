using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework.Conversion;

public class ParseFunction : NativeFunction
{
    public const string Identifier = "parse";

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        if (arguments.Count != 2)
            throw new InvalidArgumentCountException(2, arguments.Count);

        var type = await arguments[0].EvaluateAsync(interpreter, cancellationToken);
        var targetType = ValueTypeExtensions.FromTypeName(type.ExpectString().Value);

        var source = await arguments[1].EvaluateAsync(interpreter, cancellationToken);

        if (source.ResultType == targetType)
            return source;

        return source switch
        {
            StringResult stringResult => targetType switch
            {
                ResultType.Number when double.TryParse(stringResult.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var doubleValue) => new NumberResult(doubleValue),
                ResultType.Bool when bool.TryParse(stringResult.Value, out var boolValue) => new BoolResult(boolValue),
                ResultType.Bool when stringResult.Value is "1" or "0" => new BoolResult(stringResult.Value is "1"),
                _ => NullResult.Instance,
            },
            NumberResult numberResult => targetType switch
            {
                ResultType.Str => new StringResult(numberResult.Value.ToString(CultureInfo.InvariantCulture)),
                ResultType.Bool => new BoolResult(numberResult.Value != 0),
                _ => NullResult.Instance,
            },
            BoolResult boolResult => targetType switch
            {
                ResultType.Str => new StringResult(boolResult.Value ? "true" : "false"),
                ResultType.Number => new NumberResult(boolResult.Value ? 1 : 0),
                _ => NullResult.Instance,
            },
            NullResult => targetType switch
            {
                ResultType.Str => StringResult.Empty,
                ResultType.Number => NumberResult.Zero,
                ResultType.Bool => BoolResult.False,
                _ => NullResult.Instance,
            },
            _ => throw new InvalidResultTypeException(source.ResultType, ResultType.Str, ResultType.Number, ResultType.Bool, ResultType.Null),
        };
    }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    protected override string DebugParamsString => "string type, any value";
}