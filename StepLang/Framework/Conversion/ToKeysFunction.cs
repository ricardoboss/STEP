using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Framework.Conversion;

public class ToKeysFunction : NativeFunction
{
    public const string Identifier = "toKeys";

    public override IEnumerable<(ResultType[] types, string identifier)> Parameters => new[] { (new[] { ResultType.Map }, "source") };

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments);

        var map = await arguments.Single().EvaluateAsync(interpreter, r => r.ExpectMap().Value, cancellationToken);

        var keys = map.Keys.Select(k => new StringResult(k));

        return new ListResult(keys.Cast<ExpressionResult>().ToList());
    }
}