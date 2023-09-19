using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Framework.Mutating;

public class DoInsertAtFunction : NativeFunction
{
    public const string Identifier = "doInsertAt";

    public override IEnumerable<(ResultType[] types, string identifier)> Parameters => new[] { (new[] { ResultType.List }, "subject"), (new[] { ResultType.Number }, "index"), (Enum.GetValues<ResultType>(), "value") };

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments);

        var (listExpression, indexExpression, valueExpression) = (arguments[0], arguments[1], arguments[2]);

        var list = await listExpression.EvaluateAsync(interpreter, r => r.ExpectList().Value, cancellationToken);
        var index = await indexExpression.EvaluateAsync(interpreter, r => r.ExpectInteger().RoundedIntValue, cancellationToken);
        var value = await valueExpression.EvaluateAsync(interpreter, cancellationToken);

        if (index < 0 || index > list.Count)
            throw new IndexOutOfBoundsException(index, list.Count);

        list.Insert(index, value);

        return VoidResult.Instance;
    }
}