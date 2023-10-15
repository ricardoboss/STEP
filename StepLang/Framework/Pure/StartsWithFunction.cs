using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Framework.Pure;

public class StartsWithFunction : NativeFunction
{
    public const string Identifier = "startsWith";

    public override IEnumerable<(ResultType[] types, string identifier)> Parameters => new[] { (new[] { ResultType.Str }, "subject"), (new[] { ResultType.Str }, "prefix") };

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments);

        var (subjectExpression, prefixExpression) = (arguments[0], arguments[1]);

        var subject = await subjectExpression.EvaluateAsync(interpreter, r => r.ExpectString().Value, cancellationToken);
        var prefix = await prefixExpression.EvaluateAsync(interpreter, r => r.ExpectString().Value, cancellationToken);

        return new BoolResult(subject.GraphemeStartsWith(prefix));
    }
}