using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Framework.Pure;

public class InterpolateFunction : NativeFunction
{
    public const string Identifier = "interpolate";

    public override IEnumerable<(ResultType[] types, string identifier)> Parameters => new[] { (new[] { ResultType.Number }, "a"), (new[] { ResultType.Number }, "b"), (new[] { ResultType.Number }, "t") };

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments);

        var a = await arguments[0].EvaluateAsync(interpreter, r => r.ExpectNumber().Value, cancellationToken);
        var b = await arguments[1].EvaluateAsync(interpreter, r => r.ExpectNumber().Value, cancellationToken);
        var t = await arguments[2].EvaluateAsync(interpreter, r => r.ExpectNumber().Value, cancellationToken);

        var value = a + (b - a) * t;

        return new NumberResult(value);
    }
}