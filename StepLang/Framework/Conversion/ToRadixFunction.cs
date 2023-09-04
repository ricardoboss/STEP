using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework.Conversion;

public class ToRadixFunction : NativeFunction
{
    public const string Identifier = "toRadix";

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments, 2);

        var (valueExpression, radixExpression) = (arguments[0], arguments[1]);

        var value = await valueExpression.EvaluateAsync(interpreter, r => r.ExpectNumber().Value, cancellationToken);
        var radix = await radixExpression.EvaluateAsync(interpreter, r => r.ExpectInteger().RoundedIntValue, cancellationToken);

        try
        {
            var convertedValue = radix switch
            {
                2 or 8 or 10 or 16 => Convert.ToString((long)value, radix).ToUpperInvariant(),
                _ => throw new ArgumentException($"Radix {radix} is not supported."),
            };

            return new StringResult(convertedValue);
        }
        catch (ArgumentException)
        {
            return NullResult.Instance;
        }
    }

    protected override string DebugParamsString => "number value, number radix";
}