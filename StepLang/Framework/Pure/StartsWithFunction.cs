using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework.Pure;

public class StartsWithFunction : NativeFunction
{
    public const string Identifier = "startsWith";

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments, 2);

        var (subjectExpression, prefixExpression) = (arguments[0], arguments[1]);

        var subject = await subjectExpression.EvaluateAsync(interpreter, r => r.ExpectString().Value, cancellationToken);
        var prefix = await prefixExpression.EvaluateAsync(interpreter, r => r.ExpectString().Value, cancellationToken);

        return new BoolResult(subject.StartsWith(prefix, StringComparison.Ordinal));
    }

    protected override string DebugParamsString => "string subject, string prefix";
}