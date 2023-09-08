using System.Globalization;
using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework.Conversion;

public class ToStringFunction : NativeFunction
{
    public const string Identifier = "toString";

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments, 1);

        var value = await arguments.Single().EvaluateAsync(interpreter, cancellationToken);

        return new StringResult(Render(value));
    }

    protected override string DebugParamsString => "any value";

    internal static string Render(ExpressionResult result)
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
            case VoidResult:
                return "void";
            case FunctionResult:
                return "function";
            case ListResult { Value: var values }:
                var renderedValues = values.Select(v =>
                {
                    string renderedValue;
                    if (v is StringResult stringValue)
                        renderedValue = $"\"{stringValue.Value}\"";
                    else
                        renderedValue = Render(v);

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
                        renderedValue = Render(pair.Value);

                    return $"\"{pair.Key}\": {renderedValue}";
                });
                var mapContents = string.Join(", ", renderedPairs);
                return $"{{{mapContents}}}";
            default:
                throw new NotImplementedException();
        }
    }
}