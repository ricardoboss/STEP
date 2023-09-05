using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework.Pure;

public class ContainsFunction : NativeFunction
{
    public const string Identifier = "contains";

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments, 2);

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

    protected override string DebugParamsString => "list|map|string subject, any|string value";
}