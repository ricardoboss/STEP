using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;

namespace StepLang.Framework.Pure;

public class LengthFunction : NativeFunction
{
    public const string Identifier = "length";

    public override IEnumerable<(ResultType[] types, string identifier)> Parameters => new[] { (new[] { ResultType.Str, ResultType.List, ResultType.Map }, "subject") };

    protected override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments);

        var subjectResult = await arguments.Single().EvaluateAsync(interpreter, cancellationToken);

        return subjectResult switch
        {
            StringResult { Value: var str } => new NumberResult(str.GraphemeLength()),
            ListResult { Value: var list } => new NumberResult(list.Count),
            MapResult { Value: var map } => new NumberResult(map.Count),
            _ => throw new InvalidArgumentTypeException(null, Parameters.Single().types, subjectResult),
        };
    }
}