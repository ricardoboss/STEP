using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;

namespace StepLang.Framework.Pure;

public abstract class ListManipulationFunction : GenericFunction<ListResult, FunctionResult>
{
    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(OnlyList, "subject"),
        new(OnlyFunction, "callback"),
    };

    protected override ExpressionResult Invoke(Interpreter interpreter, ListResult argument1, FunctionResult argument2)
    {
        var list = argument1.DeepClone().Value;
        var callback = argument2.Value;

        var args = PrepareArgsForCallback(list, callback);
        var result = EvaluateListManipulation(interpreter, args, callback).ToList();

        return new ListResult(result);
    }

    protected virtual IEnumerable<ExpressionNode[]> PrepareArgsForCallback(IEnumerable<ExpressionResult> list, FunctionDefinition callback)
    {
        var callbackParameters = callback.Parameters.ToList();
        Func<ExpressionResult, int, ExpressionNode[]> argsConverter;

        switch (callbackParameters.Count)
        {
            case < 1 or > 2:
                throw new InvalidArgumentTypeException(null, $"Callback function must have 1 or 2 parameters, but has {callbackParameters.Count}");
            case 2:
                if (!callbackParameters[1].HasResultType(ResultType.Number))
                    throw new InvalidArgumentTypeException(null, $"Second parameter of callback function must accept numbers, but is {callbackParameters[1].ResultTypesToString()}");

                argsConverter = (element, index) =>
                {
                    var elementExpression = element.ToExpressionNode();
                    var indexExpression = (LiteralExpressionNode)index;

                    return new[] { elementExpression, indexExpression };
                };

                break;
            default:
                argsConverter = (element, _) =>
                {
                    return new[] { element.ToExpressionNode() };
                };

                break;
        }

        return list.Select(argsConverter);
    }

    protected abstract IEnumerable<ExpressionResult> EvaluateListManipulation(Interpreter interpreter, IEnumerable<ExpressionNode[]> arguments, FunctionDefinition callback);
}