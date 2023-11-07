using System.Text.Json;
using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Framework.Conversion;

public class ToJsonFunction : GenericFunction<ExpressionResult>
{
    public const string Identifier = "toJson";

    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(AnyValueType, "value"),
    };

    protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyString;

    protected override StringResult Invoke(Interpreter interpreter, ExpressionResult argument1)
    {
        return JsonSerializer.Serialize(argument1, JsonConversionContext.Default.ExpressionResult);
    }
}