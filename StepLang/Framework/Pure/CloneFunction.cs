using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework.Pure;

public class CloneFunction : NativeFunction
{
    public const string Identifier = "clone";

    public override IEnumerable<(ResultType[] types, string identifier)> Parameters => new[] { (Enum.GetValues<ResultType>(), "subject") };

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments);

        var subject = await arguments.Single().EvaluateAsync(interpreter, cancellationToken);

        return subject.DeepClone();
    }
}