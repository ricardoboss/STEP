using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;

namespace StepLang.Framework.Pure;

public class LengthFunction : NativeFunction
{
    public const string Identifier = "length";

    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new[]
    {
        new NativeParameter(new[] { ResultType.Str, ResultType.List, ResultType.Map }, "subject"),
    };

    protected override IEnumerable<ResultType> ReturnTypes { get; } = new[] { ResultType.Number };

    public override ExpressionResult Invoke(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments)
    {
        CheckArgumentCount(arguments);

        var subjectResult = arguments.Single().EvaluateUsing(interpreter);

        return subjectResult switch
        {
            StringResult { Value: var str } => new NumberResult(str.GraphemeLength()),
            ListResult { Value: var list } => new NumberResult(list.Count),
            MapResult { Value: var map } => new NumberResult(map.Count),
            _ => throw new InvalidArgumentTypeException(arguments.Single().Location, NativeParameters.Single().Types, subjectResult),
        };
    }
}