using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework.Pure;

public class IndexOfFunction : NativeFunction
{
    public const string Identifier = "indexOf";

    public override IEnumerable<(ResultType [] types, string identifier)> Parameters => new [] { (new [] { ResultType.List, ResultType.Map, ResultType.Str }, "subject"), (Enum.GetValues<ResultType>(), "value") };

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter,
        IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
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
            StringResult str => new NumberResult(str.Value.IndexOf(value.ExpectString().Value, StringComparison.Ordinal)),
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