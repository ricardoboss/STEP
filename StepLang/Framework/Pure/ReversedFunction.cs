using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;

namespace StepLang.Framework.Pure;

public class ReversedFunction : NativeFunction
{
    public const string Identifier = "reversed";

    public override IEnumerable<(ResultType[] types, string identifier)> Parameters => new[] { (new[] { ResultType.List, ResultType.Str }, "subject") };

    protected override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments);

        var subjectResult = await arguments.Single().EvaluateAsync(interpreter, cancellationToken);
        return subjectResult.ResultType switch
        {
            ResultType.List => new ListResult(subjectResult.ExpectList().DeepClone().Value.Reverse().ToList()),
            ResultType.Str => new StringResult(subjectResult.ExpectString().Value.ReverseGraphemes()),
            _ => throw new InvalidResultTypeException(subjectResult.ResultType, ResultType.List, ResultType.Str),
        };
    }
}
