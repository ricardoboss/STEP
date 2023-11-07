using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Framework.Mutating;

public class DoAddFunction : GenericFunction<ListResult, ExpressionResult>
{
    public const string Identifier = "doAdd";

    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(OnlyList, "list"),
        new(AnyValueType, "value"),
    };

    protected override ExpressionResult Invoke(Interpreter interpreter, ListResult argument1, ExpressionResult argument2)
    {
        var list = argument1;
        var value = argument2;

        list.Value.Add(value);

        return VoidResult.Instance;
    }
}