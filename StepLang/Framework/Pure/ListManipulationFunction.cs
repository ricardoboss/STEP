using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

/// <summary>
/// Base class for functions that manipulate lists.
/// </summary>
public abstract class ListManipulationFunction : GenericFunction<ListResult, FunctionResult>
{
    /// <inheritdoc />
    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(OnlyList, "subject"),
        new(OnlyFunction, "callback"),
    };

    /// <inheritdoc />
    protected override ExpressionResult Invoke(TokenLocation callLocation, Interpreter interpreter, ListResult argument1, FunctionResult argument2)
    {
        var list = argument1.DeepClone().Value;
        var callback = argument2.Value;

        var args = PrepareArgsForCallback(callLocation, list, callback);
        var result = EvaluateListManipulation(callLocation, interpreter, args, callback).ToList();

        return new ListResult(result);
    }

    /// <summary>
    /// Prepares the arguments for the callback function.
    /// </summary>
    /// <param name="callLocation">The location of the call.</param>
    /// <param name="list">The list to prepare the arguments for.</param>
    /// <param name="callback">The callback function.</param>
    /// <returns>The arguments for the callback function.</returns>
    /// <exception cref="InvalidArgumentTypeException">Thrown when the callback function has an invalid number of parameters.</exception>
    protected virtual IEnumerable<ExpressionNode[]> PrepareArgsForCallback(TokenLocation callLocation, IEnumerable<ExpressionResult> list, FunctionDefinition callback)
    {
        var callbackParameters = callback.Parameters.ToList();
        Func<ExpressionResult, int, ExpressionNode[]> argsConverter;

        switch (callbackParameters.Count)
        {
            case < 1 or > 2:
                throw new InvalidArgumentTypeException(callLocation, $"Callback function must have 1 or 2 parameters, but has {callbackParameters.Count}");
            case 2:
                if (!callbackParameters[1].HasResultType(ResultType.Number))
                    throw new InvalidArgumentTypeException(callLocation, $"Second parameter of callback function must accept numbers, but is {callbackParameters[1].ResultTypesToString()}");

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

    /// <summary>
    /// Evaluates the list manipulation.
    /// </summary>
    /// <param name="callLocation">The location of the call.</param>
    /// <param name="interpreter">The interpreter.</param>
    /// <param name="arguments">The arguments for the callback function.</param>
    /// <param name="callback">The callback function.</param>
    /// <returns>The results of the list manipulation.</returns>
    protected abstract IEnumerable<ExpressionResult> EvaluateListManipulation(TokenLocation callLocation, Interpreter interpreter, IEnumerable<ExpressionNode[]> arguments, FunctionDefinition callback);
}