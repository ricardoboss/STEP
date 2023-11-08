using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.Framework.Conversion;

public class ToNumberFunction : GenericFunction<StringResult, NumberResult>
{
    public const string Identifier = "toNumber";

    protected override NativeParameter[] NativeParameters { get; } = {
        new(OnlyString, "value"),
        new(OnlyNumber, "radix", DefaultValue: LiteralExpressionNode.FromInt32(10)),
    };

    protected override IEnumerable<ResultType> ReturnTypes => new[] { ResultType.Number, ResultType.Null };

    private TokenLocation? radixLocation;

    public override ExpressionResult Invoke(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments)
    {
        // this is a hack
        // we should introduce a way to get a token location from the invocation for better error reporting
        radixLocation = arguments[1].Location;

        return base.Invoke(interpreter, arguments);
    }

    protected override ExpressionResult Invoke(Interpreter interpreter, StringResult argument1, NumberResult argument2)
    {
        var value = argument1.Value;
        var radix = argument2;

        try
        {
            return radix.Value switch
            {
                2 or 8 or 16 => NumberResult.FromInt32(Convert.ToInt32(value, radix)),
                10 => NumberResult.FromString(value),
                _ => throw new InvalidArgumentValueException(radixLocation, $"Radix {radix} is not supported."),
            };
        }
        catch (Exception e) when (e is ArgumentException or FormatException)
        {
            return NullResult.Instance;
        }
    }
}