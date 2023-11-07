using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;

namespace StepLang.Framework.Pure;

public class SubstringFunction : NativeFunction
{
    public const string Identifier = "substring";

    protected override IEnumerable<NativeParameter> NativeParameters => new NativeParameter[] { (new[] { ResultType.Str }, "subject"), (new[] { ResultType.Number }, "start"), (new[] { ResultType.Number }, "length") };

    /// <inheritdoc />
    protected override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments, CancellationToken cancellationToken = default)
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