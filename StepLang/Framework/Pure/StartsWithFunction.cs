using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Framework.Pure;

public class StartsWithFunction : GenericFunction<StringResult, StringResult>
{
    public const string Identifier = "startsWith";

    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(OnlyString, "subject"),
        new(OnlyString, "prefix"),
    };

    protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyBool;

    protected override BoolResult Invoke(Interpreter interpreter, StringResult argument1, StringResult argument2)
    {
        return argument1.Value.GraphemeStartsWith(argument2);
    }
}