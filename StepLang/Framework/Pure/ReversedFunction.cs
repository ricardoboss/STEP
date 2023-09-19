using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Framework.Pure;

public class ReversedFunction : NativeFunction
{
    public const string Identifier = "reversed";

    public override IEnumerable<(ResultType[] types, string identifier)> Parameters => new[] { (new[] { ResultType.List }, "subject") };

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments);

        var subjectResult = await arguments.Single().EvaluateAsync(interpreter, r => r.ExpectList(), cancellationToken);
        var reversed = subjectResult.DeepClone().Value.Reverse().ToList();
        return new ListResult(reversed);
    }
}