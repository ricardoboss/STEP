using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Mutating;

/// <summary>
/// Adds a value to a list by mutating it.
/// </summary>
public class DoAddFunction : GenericFunction<ListResult, ExpressionResult>
{
    /// <summary>
    /// The identifier of the <see cref="DoAddFunction"/> function.
    /// </summary>
    public const string Identifier = "doAdd";

    /// <inheritdoc />
    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(OnlyList, "list"),
        new(AnyValueType, "value"),
    };

    /// <inheritdoc />
    protected override ExpressionResult Invoke(TokenLocation callLocation, Interpreter interpreter,
        ListResult argument1, ExpressionResult argument2)
    {
        var list = argument1;
        var value = argument2;

        list.Value.Add(value);

        return VoidResult.Instance;
    }
}