using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Mutating;

/// <summary>
/// Shifts a value from the start of a list by mutating it.
/// </summary>
public class DoShiftFunction : GenericFunction<ListResult>
{
    /// <summary>
    /// The identifier of the <see cref="DoShiftFunction"/> function.
    /// </summary>
    public const string Identifier = "doShift";

    /// <inheritdoc />
    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(OnlyList, "subject"),
    };

    /// <inheritdoc />
    protected override IEnumerable<ResultType> ReturnTypes => AnyValueType;

    /// <inheritdoc />
    protected override ExpressionResult Invoke(TokenLocation callLocation, Interpreter interpreter,
        ListResult argument1)
    {
        var list = argument1.Value;
        if (list.Count == 0)
            return NullResult.Instance;

        var value = list[0];
        list.RemoveAt(0);

        return value;
    }
}