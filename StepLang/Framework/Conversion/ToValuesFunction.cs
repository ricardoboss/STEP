using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Conversion;

public class ToValuesFunction : GenericFunction<MapResult>
{
    public const string Identifier = "toValues";

    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(OnlyMap, "source"),
    };

    protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyList;

    protected override ExpressionResult Invoke(TokenLocation callLocation, Interpreter interpreter, MapResult argument1)
    {
        var map = argument1.Value;

        return new ListResult(map.Values.ToList());
    }
}