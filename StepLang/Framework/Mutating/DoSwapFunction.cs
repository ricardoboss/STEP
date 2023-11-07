using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;

namespace StepLang.Framework.Mutating;

public class DoSwapFunction : NativeFunction
{
    public const string Identifier = "doSwap";

    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(new[] { ResultType.List, ResultType.Map }, "subject"),
        new(new[] { ResultType.Number, ResultType.Str }, "a"),
        new(new[] { ResultType.Number, ResultType.Str }, "b"),
    };

    protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyBool;

    public override BoolResult Invoke(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments)
    {
        CheckArgumentCount(arguments);

        var (subjectExpression, aExpression, bExpression) = (arguments[0], arguments[1], arguments[2]);

        var subject = subjectExpression.EvaluateUsing(interpreter);
        var a = aExpression.EvaluateUsing(interpreter);
        var b = bExpression.EvaluateUsing(interpreter);

        return subject switch
        {
            MapResult mapResult when a is StringResult aKey && b is StringResult bKey => SwapMap(mapResult, aKey, bKey),
            ListResult listResult when a is NumberResult aIndex && b is NumberResult bIndex => SwapList(listResult, aIndex, bIndex),
            // TODO improve error message if one of the parameters has the wrong type
            _ => throw new InvalidArgumentTypeException(subjectExpression.Location, NativeParameters.First().Types, subject),
        };
    }

    private static BoolResult SwapMap(MapResult map, StringResult aKey, StringResult bKey)
    {
        var aExists = map.Value.TryGetValue(aKey, out var aValue);
        var bExists = map.Value.TryGetValue(bKey, out var bValue);

        if (!aExists || !bExists)
            return BoolResult.False;

        map.Value[aKey] = bValue!;
        map.Value[bKey] = aValue!;

        return BoolResult.True;
    }

    private static BoolResult SwapList(ListResult list, NumberResult aIndex, NumberResult bIndex)
    {
        if (aIndex < 0 || aIndex >= list.Value.Count || bIndex < 0 || bIndex >= list.Value.Count)
            return BoolResult.False;

        (list.Value[aIndex], list.Value[bIndex]) = (list.Value[bIndex], list.Value[aIndex]);

        return BoolResult.True;
    }
}