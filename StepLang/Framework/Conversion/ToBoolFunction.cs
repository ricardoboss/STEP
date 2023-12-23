using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Conversion;

/// <summary>
/// Converts any value to a boolean using the <see cref="ExpressionResult.IsTruthy()"/> method.
/// </summary>
public class ToBoolFunction : GenericFunction<ExpressionResult>
{
    /// <summary>
    /// The identifier of the <see cref="ToBoolFunction"/> function.
    /// </summary>
    public const string Identifier = "toBool";

    /// <inheritdoc />
    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(AnyValueType, "value"),
    };

    /// <inheritdoc />
    protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyBool;

    /// <inheritdoc />
    protected override BoolResult Invoke(TokenLocation callLocation, Interpreter interpreter, ExpressionResult argument1) => argument1.IsTruthy();
}