using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Framework.Conversion;

public class ToValuesFunction : NativeFunction
{
    public const string Identifier = "toValues";

    public override IEnumerable<(ResultType[] types, string identifier)> Parameters => new[] { (new[] { ResultType.Map }, "source") };

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments);

        var map = await arguments.Single().EvaluateAsync(interpreter, r => r.ExpectMap().Value, cancellationToken);
        var values = map.Values.ToList();

        return new ListResult(values);
    }
}