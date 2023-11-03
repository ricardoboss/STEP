using System.Linq.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;

namespace StepLang.Framework.Pure;

public class MaxFunction : NativeFunction
{
    public const string Identifier = "max";

    protected override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments, CancellationToken cancellationToken = default)
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