using System.Globalization;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;

namespace StepLang.Framework.Conversion;

public class ToStringFunction : NativeFunction
{
    public const string Identifier = "toString";

    protected override IEnumerable<(ResultType[] types, string identifier)> NativeParameters => new[] { (Enum.GetValues<ResultType>(), "value") };

    public override IEnumerable<ResultType> ReturnTypes => new[] { ResultType.Str };

    public override ExpressionResult Invoke(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments)
    {
        CheckArgumentCount(arguments);

        var value = arguments.Single().EvaluateUsing(interpreter);

        return new StringResult(Render(value));
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
            _ => throw new NotImplementedException(),
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
