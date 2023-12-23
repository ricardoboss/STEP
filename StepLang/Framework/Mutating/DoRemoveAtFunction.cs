using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Mutating;

/// <summary>
/// Removes an element from a list at the given index by mutating it.
/// </summary>
public class DoRemoveAtFunction : GenericFunction<ListResult, NumberResult>
{
    /// <summary>
    /// The identifier of the <see cref="DoRemoveAtFunction"/> function.
    /// </summary>
    public const string Identifier = "doRemoveAt";

    /// <inheritdoc />
    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(OnlyList, "subject"),
        new(OnlyNumber, "index"),
    };

    /// <inheritdoc />
    protected override IEnumerable<ResultType> ReturnTypes { get; } = AnyValueType;

    /// <inheritdoc />
    protected override ExpressionResult Invoke(TokenLocation callLocation, Interpreter interpreter,
        ListResult argument1, NumberResult argument2)
    {
        var list = argument1.Value;
        var index = argument2;

        if (list.Count == 0 || index < 0 || index >= list.Count)
            throw new IndexOutOfBoundsException(callLocation, index, list.Count);

        var element = list[index];
        list.RemoveAt(index);

        return element;
    }
}