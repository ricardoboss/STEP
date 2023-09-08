using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework.Pure;

public class SinFunction : NativeFunction
{
    public const string Identifier = "sin";

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments, 1);

        var number = await arguments.Single().EvaluateAsync(interpreter, r => r.ExpectNumber().Value, cancellationToken);

        return new NumberResult(Math.Sin(number));
    }

    protected override string DebugParamsString => "number x";
}