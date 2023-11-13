using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Mutating;

public class DoSwapFunction : GenericFunction<ExpressionResult, ExpressionResult, ExpressionResult>
{
    public const string Identifier = "doSwap";

    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(new[] { ResultType.List, ResultType.Map }, "subject"),
        new(new[] { ResultType.Number, ResultType.Str }, "a"),
        new(new[] { ResultType.Number, ResultType.Str }, "b"),
    };

    protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyBool;

    protected override ExpressionResult Invoke(TokenLocation tokenLocation, Interpreter interpreter,
        ExpressionResult argument1, ExpressionResult argument2, ExpressionResult argument3)
    {
        return argument1 switch
        {
            MapResult mapResult when argument2 is StringResult aKey && argument3 is StringResult bKey => SwapMap(mapResult, aKey, bKey),
            ListResult listResult when argument2 is NumberResult aIndex && argument3 is NumberResult bIndex => SwapList(listResult, aIndex, bIndex),
            _ => ThrowForInvalidCombination(),
        };

        ExpressionResult ThrowForInvalidCombination()
        {
            if (argument1 is MapResult)
            {
                if (argument2 is not StringResult)
                    throw new InvalidArgumentTypeException(null, NativeParameters.ElementAt(1).Types, argument2);

                if (argument3 is not StringResult)
                    throw new InvalidArgumentTypeException(null, NativeParameters.ElementAt(2).Types, argument3);

                throw new InvalidOperationException(); // should never reach this
            }

            if (argument1 is ListResult)
            {
                if (argument2 is not NumberResult)
                    throw new InvalidArgumentTypeException(null, NativeParameters.ElementAt(1).Types, argument2);

                if (argument3 is not NumberResult)
                    throw new InvalidArgumentTypeException(null, NativeParameters.ElementAt(2).Types, argument3);

                throw new InvalidOperationException(); // should never reach this
            }

            throw new InvalidArgumentTypeException(null, NativeParameters.First().Types, argument1);
        }
    }

    private static BoolResult SwapMap(MapResult map, StringResult aKey, StringResult bKey)
    {
        var aExists = map.Value.TryGetValue(aKey.Value, out var aValue);
        var bExists = map.Value.TryGetValue(bKey.Value, out var bValue);

        if (!aExists || !bExists)
            return BoolResult.False;

        map.Value[aKey.Value] = bValue!;
        map.Value[bKey.Value] = aValue!;

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