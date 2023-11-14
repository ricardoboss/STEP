using System.Globalization;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Conversion;

public class ToStringFunction : GenericFunction<ExpressionResult>
{
    public const string Identifier = "toString";

    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(AnyValueType, "value"),
    };

    protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyString;

    protected override ExpressionResult Invoke(TokenLocation callLocation, Interpreter interpreter,
        ExpressionResult argument1)
    {
        return new StringResult(Render(argument1));
    }

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
