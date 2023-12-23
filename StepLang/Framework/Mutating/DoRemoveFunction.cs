using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Mutating;

/// <summary>
/// Removes an element from a list by mutating it.
/// </summary>
public class DoRemoveFunction : GenericFunction<ListResult, ExpressionResult>
{
    /// <summary>
    /// The identifier of the <see cref="DoRemoveFunction"/> function.
    /// </summary>
    public const string Identifier = "doRemove";

    /// <inheritdoc />
    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(OnlyList, "subject"),
        new(AnyValueType, "element"),
    };

    /// <inheritdoc />
    protected override ExpressionResult Invoke(TokenLocation callLocation, Interpreter interpreter,
        ListResult argument1, ExpressionResult argument2)
    {
        var list = argument1.Value;
        var element = argument2;

        list.Remove(element);

        return VoidResult.Instance;
    }
}