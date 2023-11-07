using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;

namespace StepLang.Framework.Pure;

public class CloneFunction : NativeFunction
{
    public const string Identifier = "clone";

    protected override IEnumerable<NativeParameter> NativeParameters => new NativeParameter[] { (Enum.GetValues<ResultType>(), "subject") };

    protected override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments);

        var subject = await arguments.Single().EvaluateAsync(interpreter, cancellationToken);

        return subject.DeepClone();
    }
}