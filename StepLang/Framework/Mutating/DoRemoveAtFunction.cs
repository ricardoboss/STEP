using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Mutating;

public class DoRemoveAtFunction : GenericFunction<ListResult, NumberResult>
{
    public const string Identifier = "doRemoveAt";

    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(OnlyList, "subject"),
        new(OnlyNumber, "index"),
    };

    protected override IEnumerable<ResultType> ReturnTypes { get; } = AnyValueType;

    protected override ExpressionResult Invoke(TokenLocation callLocation, Interpreter interpreter,
        ListResult argument1, NumberResult argument2)
    {
        var list = argument1.Value;
        var index = argument2;

        if (list.Count == 0 || index < 0 || index >= list.Count)
            throw new IndexOutOfBoundsException(index, list.Count);

        var element = list[index];
        list.RemoveAt(index);

        return element;
    }
}