using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

public class CompareToFunction : GenericFunction<ExpressionResult, ExpressionResult>
{
    public const string Identifier = "compareTo";

    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(AnyType, "a"),
        new(AnyType, "b"),
    };

    protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyNumber;

    protected override ExpressionResult Invoke(TokenLocation callLocation, Interpreter interpreter,
        ExpressionResult argument1, ExpressionResult argument2)
    {
        return GetResult(callLocation, argument1, argument2);
    }

    private static NumberResult GetResult(TokenLocation evaluationLocation, ExpressionResult a, ExpressionResult b)
    {
        return a switch
        {
            StringResult aStr when b is StringResult bStr => aStr.CompareTo(bStr),
            NumberResult aNum when b is NumberResult bNum => aNum.CompareTo(bNum),
            BoolResult aBool when b is BoolResult bBool => aBool.Value.CompareTo(bBool.Value),
            ListResult aList when b is ListResult bList => aList.Value.Count.CompareTo(bList.Value.Count),
            MapResult aMap when b is MapResult bMap => aMap.Value.Count.CompareTo(bMap.Value.Count),
            FunctionResult when b is FunctionResult => 0,
            NullResult when b is NullResult => 0,
            VoidResult when b is VoidResult => 0,
            { ResultType: var aType } when b.ResultType != aType => throw new InvalidResultTypeException(evaluationLocation, b, a.ResultType),
            _ => throw new NotSupportedException("Unknown expression result type"),
        };
    }
}
