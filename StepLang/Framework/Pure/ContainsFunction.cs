using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;

namespace StepLang.Framework.Pure;

public class ContainsFunction : NativeFunction
{
    public const string Identifier = "contains";

    protected override IEnumerable<NativeParameter> NativeParameters => new NativeParameter[] { (new[] { ResultType.List, ResultType.Map, ResultType.Str }, "subject"), (Enum.GetValues<ResultType>(), "value") };

    protected override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments);

        var (subjectExpression, valueExpression) = (arguments[0], arguments[1]);

        var subject = await subjectExpression.EvaluateAsync(interpreter, cancellationToken);
        var value = await valueExpression.EvaluateAsync(interpreter, cancellationToken);

        var result = IndexOfFunction.GetResult(subject, value);

        return result switch
        {
            NumberResult { Value: >= 0 } => BoolResult.True,
            StringResult => BoolResult.True,
            _ => BoolResult.False,
        };
    }
}