using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Conversion;

/// <summary>
/// Converts the given <see cref="NumberResult"/> value to a <see cref="StringResult"/> with the given radix.
/// </summary>
public class ToRadixFunction : GenericFunction<NumberResult, NumberResult>
{
    /// <summary>
    /// The identifier of the <see cref="ToRadixFunction"/> function.
    /// </summary>
    public const string Identifier = "toRadix";

    /// <inheritdoc />
    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(OnlyNumber, "value"),
        new(OnlyNumber, "radix"),
    };

    /// <inheritdoc />
    protected override IEnumerable<ResultType> ReturnTypes { get; } = NullableString;

    /// <inheritdoc />
    protected override ExpressionResult Invoke(TokenLocation callLocation, Interpreter interpreter,
        NumberResult argument1, NumberResult argument2)
    {
        var number = argument1;
        var radix = argument2;

        try
        {
            return radix.Value switch
            {
                2 or 8 or 10 or 16 => (StringResult)Convert.ToString(number.ToUInt32(), radix).ToUpperInvariant(),
                _ => NullResult.Instance,
            };
        }
        catch (ArgumentException)
        {
            return NullResult.Instance;
        }
    }
}