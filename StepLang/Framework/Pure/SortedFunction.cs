using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

public class SortedFunction : ListManipulationFunction
{
    public const string Identifier = "sorted";

    public override ExpressionResult Invoke(TokenLocation callLocation, Interpreter interpreter,
        IReadOnlyList<ExpressionNode> arguments)
    {
        CheckArgumentCount(callLocation, arguments, 1, 2);

        if (arguments.Count == 2)
            return base.Invoke(callLocation, interpreter, arguments);

        return base.Invoke(callLocation, interpreter, new[] { arguments[0], new CompareToFunction().ToResult().ToExpressionNode() });
    }

    protected override IEnumerable<ExpressionNode[]> PrepareArgsForCallback(IEnumerable<ExpressionResult> list, FunctionDefinition callback)
    {
        var callbackParameters = callback.Parameters.ToList();
        if (callbackParameters.Count != 2)
            throw new InvalidArgumentTypeException(null, $"Callback function must have 2 parameters, but has {callbackParameters.Count}");

        if (!callbackParameters[0].GetResultTypes().SequenceEqual(callbackParameters[1].GetResultTypes()))
            throw new InvalidArgumentTypeException(null, $"Both parameters of callback function must have the same type, but are {callbackParameters[0].ResultTypesToString()} and {callbackParameters[1].ResultTypesToString()}");

        return list.Select(e => new[] { e.ToExpressionNode() });
    }

    protected override IEnumerable<ExpressionResult> EvaluateListManipulation(TokenLocation callLocation, Interpreter interpreter, IEnumerable<ExpressionNode[]> arguments, FunctionDefinition callback)
    {
        var arr = arguments.ToArray();

        Array.Sort(arr, (a, b) =>
        {
            var args = new[] { a[0], b[0] };

            var result = callback.Invoke(callLocation, interpreter, args);
            if (result is not NumberResult numberResult)
                throw new InvalidResultTypeException(callLocation, result, ResultType.Number);

            return Math.Sign(numberResult);
        });

        foreach (var arg in arr)
            yield return arg[0].EvaluateUsing(interpreter);
    }
}