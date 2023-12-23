using System.Globalization;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Conversion;

/// <summary>
/// Converts any value to a string using the <see cref="Render"/> method.
/// </summary>
public class ToStringFunction : GenericFunction<ExpressionResult>
{
    /// <summary>
    /// The identifier of the <see cref="ToStringFunction"/> function.
    /// </summary>
    public const string Identifier = "toString";

    /// <inheritdoc />
    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(AnyValueType, "value"),
    };

    /// <inheritdoc />
    protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyString;

    /// <inheritdoc />
    protected override ExpressionResult Invoke(TokenLocation callLocation, Interpreter interpreter,
        ExpressionResult argument1)
    {
        return new StringResult(Render(argument1));
    }

    /// <summary>
    /// Takes an <see cref="ExpressionResult"/> and renders it as a string.
    /// </summary>
    /// <param name="result">The result to render.</param>
    /// <returns>The rendered string.</returns>
    /// <exception cref="NotSupportedException">Thrown when the given result type is not supported.</exception>
    internal static string Render(ExpressionResult result)
    {
        return result switch
        {
            StringResult { Value: var stringValue } => stringValue,
            NumberResult { Value: var numberValue } => numberValue.ToString(CultureInfo.InvariantCulture),
            BoolResult { Value: var boolValue } => boolValue.ToString(),
            NullResult => "null",
            VoidResult => "void",
            FunctionResult => "function",
            ListResult list => RenderList(list),
            MapResult map => RenderMap(map),
            _ => throw new NotSupportedException("Unknown expression result type"),
        };
    }

    private static string RenderList(ListResult result)
    {
        var renderedValues = result.Value.Select(v =>
        {
            string renderedValue;
            if (v is StringResult stringValue)
                renderedValue = $"\"{stringValue.Value}\"";
            else
                renderedValue = Render(v);

            return renderedValue;
        });

        return $"[{string.Join(", ", renderedValues)}]";
    }

    private static string RenderMap(MapResult result)
    {
        var renderedPairs = result.Value.Select(pair =>
        {
            string renderedValue;
            if (pair.Value is StringResult stringValue)
                renderedValue = $"\"{stringValue.Value}\"";
            else
                renderedValue = Render(pair.Value);

            return $"\"{pair.Key}\": {renderedValue}";
        });

        var mapContents = string.Join(", ", renderedPairs);

        return $"{{{mapContents}}}";
    }
}
