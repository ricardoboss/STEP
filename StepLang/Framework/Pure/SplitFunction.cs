using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Framework.Pure;

public class SplitFunction : NativeFunction
{
    public const string Identifier = "split";

    public override IEnumerable<(ResultType[] types, string identifier)> Parameters => new[] { (new[] { ResultType.Str }, "source"), (new[] { ResultType.Str }, "separator") };

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments, 1, 2);

        var source = await arguments[0].EvaluateAsync(interpreter, r => r.ExpectString().Value, cancellationToken);
        var separator = arguments.Count == 2 ? await arguments[1].EvaluateAsync(interpreter, r => r.ExpectString().Value, cancellationToken) : "";

        return new ListResult(source.GraphemeSplit(separator).Select(s => new StringResult(s)).Cast<ExpressionResult>().ToList());
    }
}