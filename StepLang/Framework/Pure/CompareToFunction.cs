using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;

namespace StepLang.Framework.Pure;

public class CompareToFunction : NativeFunction
{
    public const string Identifier = "compareTo";

    protected override IEnumerable<NativeParameter> NativeParameters => new NativeParameter[] { (Enum.GetValues<ResultType>(), "a"), (Enum.GetValues<ResultType>(), "b") };

    protected override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments);

        var (aExpression, bExpression) = (arguments[0], arguments[1]);

        var a = await aExpression.EvaluateAsync(interpreter, cancellationToken);
        var b = await bExpression.EvaluateAsync(interpreter, cancellationToken);

        var comparison = a switch
        {
            StringResult aStr when b is StringResult bStr => aStr.CompareTo(bStr),
            NumberResult aNum when b is NumberResult bNum => aNum.CompareTo(bNum),
            BoolResult aBool when b is BoolResult bBool => aBool.Value.CompareTo(bBool.Value),
            ListResult aList when b is ListResult bList => aList.Value.Count.CompareTo(bList.Value.Count),
            MapResult aMap when b is MapResult bMap => aMap.Value.Count.CompareTo(bMap.Value.Count),
            FunctionResult when b is FunctionResult => 0,
            NullResult when b is NullResult => 0,
            VoidResult when b is VoidResult => 0,
            { ResultType: var aType } when b.ResultType != aType => throw new InvalidResultTypeException(b, a.ResultType),
            _ => throw new NotImplementedException(),
        };

        return new NumberResult(comparison);
    }
}
