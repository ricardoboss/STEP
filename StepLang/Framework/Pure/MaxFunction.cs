using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework.Pure;

public class MaxFunction : NativeFunction
{
    public const string Identifier = "max";

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments, 1, int.MaxValue);

        var max = await arguments
            .ToAsyncEnumerable()
            .MaxAwaitAsync(async e => await Eval(e), cancellationToken);

        return new NumberResult(max);

        Task<double> Eval(Expression e) => e.EvaluateAsync(interpreter, r => r.ExpectNumber().Value, cancellationToken);
    }

    protected override string DebugParamsString => "number ...x";
}