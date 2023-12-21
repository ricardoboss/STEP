using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

public class ContainsKeyFunction : GenericFunction<MapResult, StringResult>
{
    public const string Identifier = "containsKey";

    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[] {
        new(OnlyMap, "subject"),
        new(OnlyString, "key"),
    };

    protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyBool;

    protected override BoolResult Invoke(TokenLocation callLocation, Interpreter interpreter, MapResult argument1, StringResult argument2)
    {
        return argument1.Value.ContainsKey(argument2.Value);
    }
}
