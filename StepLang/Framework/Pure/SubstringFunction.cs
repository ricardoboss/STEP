using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Framework.Pure;

public class SubstringFunction : NativeFunction
{
    public const string Identifier = "substring";

    public override IEnumerable<(ResultType[] types, string identifier)> Parameters => new[] { (new[] { ResultType.Str }, "subject"), (new[] { ResultType.Number }, "start"), (new[] { ResultType.Number }, "length") };

    /// <inheritdoc />
    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments);

        var (subjectExp, startExp, lengthExp) = (arguments[0], arguments[1], arguments[2]);

        var subject = await subjectExp.EvaluateAsync(interpreter, r => r.ExpectString().Value, cancellationToken);
        var start = await startExp.EvaluateAsync(interpreter, r => r.ExpectNumber().RoundedIntValue, cancellationToken);
        var length = await lengthExp.EvaluateAsync(interpreter, r => r.ExpectNumber().RoundedIntValue, cancellationToken);

        var substring = subject.GraphemeSubstring(start, length);

        return new StringResult(substring);
    }
}