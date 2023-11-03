using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Framework.Mutating;

public class DoShiftFunction : NativeFunction
{
    public const string Identifier = "doShift";

    public override IEnumerable<(ResultType[] types, string identifier)> Parameters => new[] { (new[] { ResultType.List }, "subject") };

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments);

        var list = await arguments.Single().EvaluateAsync(interpreter, r => r.ExpectList().Value, cancellationToken);
        if (list.Count == 0)
            return NullResult.Instance;

        var value = list[0];
        list.RemoveAt(0);

        return value;
    }
}