using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

/// <summary>
/// Returns the number, clamped between the minimum and maximum values (inclusive).
/// </summary>
public class ClampFunction : GenericFunction<NumberResult, NumberResult, NumberResult>
{
    /// <summary>
    /// The identifier of the <see cref="ClampFunction"/> function.
    /// </summary>
    public const string Identifier = "clamp";

    /// <inheritdoc />
    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(OnlyNumber, "min"),
        new(OnlyNumber, "max"),
        new(OnlyNumber, "x"),
    };

    /// <inheritdoc />
    protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyNumber;

    /// <inheritdoc />
    protected override ExpressionResult Invoke(TokenLocation callLocation, Interpreter interpreter,
        NumberResult argument1, NumberResult argument2, NumberResult argument3)
    {
        var min = argument1;
        var max = argument2;
        var x = argument3;

        return new NumberResult(Math.Max(min, Math.Min(max, x)));
    }
}