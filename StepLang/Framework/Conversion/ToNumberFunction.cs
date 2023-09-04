using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework.Conversion;

public class ToNumberFunction : NativeFunction
{
    public const string Identifier = "toNumber";

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments, 1, 2);

        var value = await arguments[0].EvaluateAsync(interpreter, r => r.ExpectString().Value, cancellationToken);

        var radix = 10;
        if (arguments.Count == 2)
            radix = await arguments[1].EvaluateAsync(interpreter, r => r.ExpectInteger().RoundedIntValue, cancellationToken);

        try
        {
            var result = radix switch
            {
                2 or 8 or 10 or 16 => Convert.ToInt32(value, radix),
                _ => throw new ArgumentException($"Radix {radix} is not supported."),
            };

            return new NumberResult(result);
        }
        catch (ArgumentException)
        {
            return NullResult.Instance;
        }
    }

    protected override string DebugParamsString => "string value, int radix = 10";
}