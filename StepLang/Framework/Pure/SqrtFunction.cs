using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

/// <summary>
/// The square root function.
/// </summary>
public class SqrtFunction : GenericFunction<NumberResult>
{
    /// <summary>
    /// The identifier of the <see cref="SqrtFunction"/> function.
    /// </summary>
    public const string Identifier = "sqrt";

    /// <inheritdoc />
    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[] { new(OnlyNumber, "x") };

    /// <inheritdoc />
    protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyNumber;

    /// <inheritdoc />
    protected override NumberResult Invoke(TokenLocation callLocation, Interpreter interpreter, NumberResult argument1) => Math.Sqrt(argument1);
}