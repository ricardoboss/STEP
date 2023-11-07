using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;

namespace StepLang.Framework.Pure;

public class IndexOfFunction : NativeFunction
{
    public const string Identifier = "indexOf";

    protected override IEnumerable<NativeParameter> NativeParameters => new NativeParameter[] { (new[] { ResultType.List, ResultType.Map, ResultType.Str }, "subject"), (Enum.GetValues<ResultType>(), "value") };

    protected override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter,
        IReadOnlyList<ExpressionNode> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments);

        var (subjectExpression, valueExpression) = (arguments[0], arguments[1]);

        var subject = await subjectExpression.EvaluateAsync(interpreter, cancellationToken);
        var value = await valueExpression.EvaluateAsync(interpreter, cancellationToken);

        return GetResult(subject, value);
    }

    internal static ExpressionResult GetResult(ExpressionResult subject, ExpressionResult value)
    {
        return subject switch
        {
            ListResult list => new NumberResult(list.Value.IndexOf(value)),
            MapResult map => GetMapKey(map, value),
            StringResult str => new NumberResult(str.Value.GraphemeIndexOf(value.ExpectString().Value)),
            _ => NullResult.Instance,
        };
    }

    private static ExpressionResult GetMapKey(MapResult map, ExpressionResult value)
    {
        var pair = map.Value.FirstOrDefault(x => x.Value.Equals(value));

        return pair.Equals(default(KeyValuePair<string, ExpressionResult>)) ?
            NullResult.Instance :
            new StringResult(pair.Key);
    }
}