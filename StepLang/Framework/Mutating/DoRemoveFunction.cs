using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Framework.Mutating;

public class DoRemoveFunction : GenericFunction<ListResult, ExpressionResult>
{
    public const string Identifier = "doRemove";

    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(OnlyList, "subject"),
        new(AnyValueType, "element"),
    };

    protected override VoidResult Invoke(Interpreter interpreter, ListResult argument1, ExpressionResult argument2)
    {
        var list = argument1.Value;
        var element = argument2;

        list.Remove(element);

        return VoidResult.Instance;
    }
}