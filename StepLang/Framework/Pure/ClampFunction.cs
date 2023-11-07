using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;

namespace StepLang.Framework.Pure;

public class ClampFunction : NativeFunction
{
    public const string Identifier = "clamp";

    protected override IEnumerable<NativeParameter> NativeParameters => new NativeParameter[] { (new[] { ResultType.Number }, "min"), (new[] { ResultType.Number }, "max"), (new[] { ResultType.Number }, "x") };

    protected override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments);

        var min = await arguments[0].EvaluateAsync(interpreter, r => r.ExpectNumber().Value, cancellationToken);
        var max = await arguments[1].EvaluateAsync(interpreter, r => r.ExpectNumber().Value, cancellationToken);
        var x = await arguments[2].EvaluateAsync(interpreter, r => r.ExpectNumber().Value, cancellationToken);

        return new NumberResult(Math.Max(min, Math.Min(max, x)));
    }
}