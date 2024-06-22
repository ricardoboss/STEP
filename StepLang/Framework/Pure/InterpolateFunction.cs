using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

/// <summary>
/// Returns the linear interpolation between two numbers.
/// </summary>
public class InterpolateFunction : GenericFunction<NumberResult, NumberResult, NumberResult>
{
    /// <summary>
    /// The identifier for the <see cref="InterpolateFunction"/> function.
    /// </summary>
    public const string Identifier = "interpolate";

    /// <inheritdoc />
    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(OnlyNumber, "a"),
        new(OnlyNumber, "b"),
        new(OnlyNumber, "t"),
    };

    /// <inheritdoc />
    protected override ExpressionResult Invoke(TokenLocation callLocation, Interpreter interpreter,
        NumberResult argument1, NumberResult argument2, NumberResult argument3)
    {
        var a = argument1;
        var b = argument2;
        var t = argument3;

        return a + (b - a) * t;
    }
}