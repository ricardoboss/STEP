using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework.Pure;

public class LengthFunction : NativeFunction
{
    public const string Identifier = "length";

    public override IEnumerable<(ResultType[] types, string identifier)> Parameters => new[] { (new[] { ResultType.Str, ResultType.List, ResultType.Map }, "subject") };

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments);

        var subjectResult = await arguments.Single().EvaluateAsync(interpreter, cancellationToken);

        return subjectResult switch
        {
            StringResult { Value: var str } => new NumberResult(str.Length),
            ListResult { Value: var list } => new NumberResult(list.Count),
            MapResult { Value: var map } => new NumberResult(map.Count),
            _ => throw new InvalidArgumentTypeException(null, Parameters.Single().types, subjectResult),
        };
    }

    protected override string DebugParamsString => "a string, list, or map";
}