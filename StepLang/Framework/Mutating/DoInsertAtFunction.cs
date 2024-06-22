using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Mutating;

/// <summary>
/// Inserts a value into a list at a given index by mutating it.
/// </summary>
public class DoInsertAtFunction : GenericFunction<ListResult, NumberResult, ExpressionResult>
{
    /// <summary>
    /// The identifier of the <see cref="DoInsertAtFunction"/> function.
    /// </summary>
    public const string Identifier = "doInsertAt";

    /// <inheritdoc />
    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(OnlyList, "list"),
        new(OnlyNumber, "index"),
        new(AnyValueType, "value"),
    };

    /// <inheritdoc />
    protected override ExpressionResult Invoke(TokenLocation callLocation, Interpreter interpreter,
        ListResult argument1, NumberResult argument2, ExpressionResult argument3)
    {
        var list = argument1.Value;
        var index = argument2;
        var value = argument3;

        if (index < 0 || index > list.Count)
            throw new IndexOutOfBoundsException(callLocation, index, list.Count);

        list.Insert(index, value);

        return VoidResult.Instance;
    }
}