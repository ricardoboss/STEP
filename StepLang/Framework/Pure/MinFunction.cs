using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework.Pure;

public class MinFunction : NativeFunction
{
    public const string Identifier = "min";

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments, 1, int.MaxValue);

        var min = await arguments
            .ToAsyncEnumerable()
            .MinAwaitAsync(async e => await Eval(e), cancellationToken);

        return new NumberResult(min);

        Task<double> Eval(Expression e) => e.EvaluateAsync(interpreter, r => r.ExpectNumber().Value, cancellationToken);
    }

    protected override string DebugParamsString => "number ...x";
}