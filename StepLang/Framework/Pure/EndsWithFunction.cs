using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework.Pure;

public class EndsWithFunction : NativeFunction
{
    public const string Identifier = "endsWith";

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments, 2);

        var (subjectExpression, prefixExpression) = (arguments[0], arguments[1]);

        var subject = await subjectExpression.EvaluateAsync(interpreter, r => r.ExpectString().Value, cancellationToken);
        var suffix = await prefixExpression.EvaluateAsync(interpreter, r => r.ExpectString().Value, cancellationToken);

        return new BoolResult(subject.EndsWith(suffix, StringComparison.Ordinal));
    }

    protected override string DebugParamsString => "string subject, string suffix";
}