using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Framework.Mutating;

public class DoAddFunction : NativeFunction
{
    public const string Identifier = "doAdd";

    public override IEnumerable<(ResultType[] types, string identifier)> Parameters => new[] { (new[] { ResultType.List }, "subject"), (Enum.GetValues<ResultType>(), "element") };

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments);

        var (listExpression, elementExpression) = (arguments[0], arguments[1]);

        var list = await listExpression.EvaluateAsync(interpreter, r => r.ExpectList().Value, cancellationToken);
        var element = await elementExpression.EvaluateAsync(interpreter, cancellationToken);

        list.Add(element);

        return VoidResult.Instance;
    }
}