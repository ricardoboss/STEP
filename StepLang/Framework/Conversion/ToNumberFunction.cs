using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.Framework.Conversion;

/// <summary>
/// Converts the given <see cref="StringResult"/> value to a <see cref="NumberResult"/>.
/// </summary>
public class ToNumberFunction : GenericFunction<StringResult, NumberResult>
{
    /// <summary>
    /// The identifier of the <see cref="ToNumberFunction"/> function.
    /// </summary>
    public const string Identifier = "toNumber";

    /// <inheritdoc />
    protected override NativeParameter[] NativeParameters { get; } = {
        new(OnlyString, "value"),
        new(OnlyNumber, "radix", DefaultValue: LiteralExpressionNode.FromInt32(10)),
    };

    /// <inheritdoc />
    protected override IEnumerable<ResultType> ReturnTypes { get; } = NullableNumber;

    /// <inheritdoc />
    protected override ExpressionResult Invoke(TokenLocation callLocation, Interpreter interpreter,
        StringResult argument1, NumberResult argument2)
    {
        var value = argument1.Value;
        var radix = argument2;

        try
        {
            return radix.Value switch
            {
                2 or 8 or 16 => NumberResult.FromInt32(Convert.ToInt32(value, radix)),
                10 => NumberResult.FromString(value),
                _ => NullResult.Instance,
            };
        }
        catch (Exception e) when (e is ArgumentException or FormatException)
        {
            return NullResult.Instance;
        }
    }
}