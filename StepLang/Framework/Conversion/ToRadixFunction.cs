using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Framework.Conversion;

public class ToRadixFunction : GenericFunction<NumberResult, NumberResult>
{
    public const string Identifier = "toRadix";

    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(OnlyNumber, "value"),
        new(OnlyNumber, "radix"),
    };

    protected override IEnumerable<ResultType> ReturnTypes { get; } = NullableString;

    protected override ExpressionResult Invoke(Interpreter interpreter, NumberResult argument1, NumberResult argument2)
    {
        var number = argument1;
        var radix = argument2;

        try
        {
            return radix.Value switch
            {
                2 or 8 or 10 or 16 => (StringResult)Convert.ToString((long)number, radix).ToUpperInvariant(),
                _ => NullResult.Instance,
            };
        }
        catch (ArgumentException)
        {
            return NullResult.Instance;
        }
    }
}