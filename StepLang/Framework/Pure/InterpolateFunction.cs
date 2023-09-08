using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework.Pure;

public class InterpolateFunction : NativeFunction
{
    public const string Identifier = "interpolate";

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments, 3);

        var a = await arguments[0].EvaluateAsync(interpreter, r => r.ExpectNumber().Value, cancellationToken);
        var b = await arguments[1].EvaluateAsync(interpreter, r => r.ExpectNumber().Value, cancellationToken);
        var t = await arguments[2].EvaluateAsync(interpreter, r => r.ExpectNumber().Value, cancellationToken);

        var value = a + (b - a) * t;

        return new NumberResult(value);
    }

    protected override string DebugParamsString => "number a, number b, number t";
}