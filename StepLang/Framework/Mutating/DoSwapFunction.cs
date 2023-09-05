using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework.Mutating;

public class DoSwapFunction : NativeFunction
{
    public const string Identifier = "doSwap";

    public override IEnumerable<(ResultType[] types, string identifier)> Parameters => new[] { (new[] { ResultType.List, ResultType.Map }, "subject"), (new[] { ResultType.Number, ResultType.Str }, "a"), (new[] { ResultType.Number, ResultType.Str }, "b") };

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments);

        var (subjectExpression, aExpression, bExpression) = (arguments[0], arguments[1], arguments[2]);

        var subject = await subjectExpression.EvaluateAsync(interpreter, cancellationToken);
        var a = await aExpression.EvaluateAsync(interpreter, cancellationToken);
        var b = await bExpression.EvaluateAsync(interpreter, cancellationToken);

        return subject switch
        {
            MapResult mapResult => SwapMap(mapResult, a, b),
            ListResult listResult => SwapList(listResult, a, b),
            _ => throw new InvalidArgumentTypeException(null, new[] { ResultType.List, ResultType.Map }, subject),
        };
    }

    private static ExpressionResult SwapMap(MapResult map, ExpressionResult a, ExpressionResult b)
    {
        var aKey = a.ExpectString().Value;
        var bKey = b.ExpectString().Value;

        var aExists = map.Value.TryGetValue(aKey, out var aValue);
        var bExists = map.Value.TryGetValue(bKey, out var bValue);

        if (!aExists || !bExists)
            return BoolResult.False;

        map.Value[aKey] = bValue!;
        map.Value[bKey] = aValue!;

        return BoolResult.True;
    }

    private static ExpressionResult SwapList(ListResult list, ExpressionResult a, ExpressionResult b)
    {
        var aIndex = a.ExpectInteger().RoundedIntValue;
        var bIndex = b.ExpectInteger().RoundedIntValue;

        if (aIndex < 0 || aIndex >= list.Value.Count || bIndex < 0 || bIndex >= list.Value.Count)
            return BoolResult.False;

        (list.Value[aIndex], list.Value[bIndex]) = (list.Value[bIndex], list.Value[aIndex]);

        return BoolResult.True;
    }
}