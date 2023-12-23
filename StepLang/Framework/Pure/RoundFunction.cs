using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

/// <summary>
/// Rounds a number to the nearest integer, using the <see cref="Math.Round(double)"/> method.
/// </summary>
public class RoundFunction : GenericFunction<NumberResult>
{
    /// <summary>
    /// The identifier of the <see cref="RoundFunction"/> function.
    /// </summary>
    public const string Identifier = "round";

    /// <inheritdoc />
    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[] { new(OnlyNumber, "x") };

    /// <inheritdoc />
    protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyNumber;

    /// <inheritdoc />
    protected override NumberResult Invoke(TokenLocation callLocation, Interpreter interpreter, NumberResult argument1) => Math.Round(argument1.Value);
}