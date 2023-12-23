using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

/// <summary>
/// Returns the ceiling of a number.
/// </summary>
public class CeilFunction : GenericFunction<NumberResult>
{
    /// <summary>
    /// The identifier of the <see cref="CeilFunction"/> function.
    /// </summary>
    public const string Identifier = "ceil";

    /// <inheritdoc />
    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[] { new(OnlyNumber, "x") };

    /// <inheritdoc />
    protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyNumber;

    /// <inheritdoc />
    protected override NumberResult Invoke(TokenLocation callLocation, Interpreter interpreter, NumberResult argument1) => Math.Ceiling(argument1.Value);
}