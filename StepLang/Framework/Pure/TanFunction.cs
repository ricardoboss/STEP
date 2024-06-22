using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

/// <summary>
/// Returns the tangent of the given angle.
/// </summary>
public class TanFunction : GenericFunction<NumberResult>
{
    /// <summary>
    /// The identifier of the <see cref="TanFunction"/> function.
    /// </summary>
    public const string Identifier = "tan";

    /// <inheritdoc />
    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[] { new(OnlyNumber, "x") };

    /// <inheritdoc />
    protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyNumber;

    /// <inheritdoc />
    protected override NumberResult Invoke(TokenLocation callLocation, Interpreter interpreter, NumberResult argument1) => Math.Tan(argument1);
}