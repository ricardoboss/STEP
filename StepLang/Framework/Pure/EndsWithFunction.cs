using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;

namespace StepLang.Framework.Pure;

public class EndsWithFunction : NativeFunction
{
    public const string Identifier = "endsWith";

    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[] { (new[] { ResultType.Str }, "subject"), (new[] { ResultType.Str }, "suffix") };

    protected override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments);

        var (subjectExpression, prefixExpression) = (arguments[0], arguments[1]);

        var subject = await subjectExpression.EvaluateAsync(interpreter, r => r.ExpectString().Value, cancellationToken);
        var suffix = await prefixExpression.EvaluateAsync(interpreter, r => r.ExpectString().Value, cancellationToken);

        return new BoolResult(subject.GraphemeEndsWith(suffix));
    }
}