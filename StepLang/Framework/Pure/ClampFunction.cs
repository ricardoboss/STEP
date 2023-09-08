using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework.Pure;

public class ClampFunction : NativeFunction
{
    public const string Identifier = "clamp";

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments, 3);

        var min = await arguments[0].EvaluateAsync(interpreter, r => r.ExpectNumber().Value, cancellationToken);
        var max = await arguments[1].EvaluateAsync(interpreter, r => r.ExpectNumber().Value, cancellationToken);
        var x = await arguments[2].EvaluateAsync(interpreter, r => r.ExpectNumber().Value, cancellationToken);

        return new NumberResult(Math.Max(min, Math.Min(max, x)));
    }

    protected override string DebugParamsString => "number min, number max, number x";
}