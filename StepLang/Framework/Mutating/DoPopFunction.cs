using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Mutating;

/// <summary>
/// Pops a value from the end of a list by mutating it.
/// </summary>
public class DoPopFunction : GenericFunction<ListResult>
{
    /// <summary>
    /// The identifier of the <see cref="DoPopFunction"/> function.
    /// </summary>
    public const string Identifier = "doPop";

    /// <inheritdoc />
    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(OnlyList, "subject"),
    };

    /// <inheritdoc />
    protected override IEnumerable<ResultType> ReturnTypes { get; } = AnyValueType;

    /// <inheritdoc />
    protected override ExpressionResult Invoke(TokenLocation callLocation, Interpreter interpreter,
        ListResult argument1)
    {
        var list = argument1.Value;

        if (list.Count == 0)
            return NullResult.Instance;

        var value = list.Last();

        list.RemoveAt(list.Count - 1);

        return value;
    }
}