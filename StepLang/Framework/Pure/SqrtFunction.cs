using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;

namespace StepLang.Framework.Pure;

public class SqrtFunction : NativeFunction
{
    public const string Identifier = "sqrt";

    protected override IEnumerable<NativeParameter> NativeParameters => new NativeParameter[] { (new[] { ResultType.Number }, "x") };

    protected override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments);

        var x = await arguments.Single().EvaluateAsync(interpreter, r => r.ExpectNumber().Value, cancellationToken);

        return new NumberResult(Math.Sqrt(x));
    }
}