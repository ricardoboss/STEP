using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Framework.Pure;

public abstract class ListManipulationFunction : NativeFunction
{
    public override IEnumerable<(ResultType[] types, string identifier)> Parameters => new[] { (new[] { ResultType.List }, "subject"), (new[] { ResultType.Function }, "callback") };

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments);

        var (subjectExpression, predicateExpression) = (arguments[0], arguments[1]);

        var subjectResult = await subjectExpression.EvaluateAsync(interpreter, r => r.ExpectList(), cancellationToken);
        var callback = await predicateExpression.EvaluateAsync(interpreter, r => r.ExpectFunction().Value, cancellationToken);

        var list = subjectResult.DeepClone().Value;
        var args = PrepareArgsForCallback(list, callback);

        var result = await EvaluateListManipulationAsync(interpreter, args, callback, cancellationToken)
            .ToListAsync(cancellationToken);

        return new ListResult(result);
    }

    protected virtual IAsyncEnumerable<ConstantExpression[]> PrepareArgsForCallback(IEnumerable<ExpressionResult> list, FunctionDefinition callback)
    {
        var callbackParameters = callback.Parameters.ToList();
        Func<ExpressionResult, int, ConstantExpression[]> argsConverter;

        switch (callbackParameters.Count)
        {
            case < 1 or > 2:
                throw new InvalidArgumentTypeException(null, $"Callback function must have 1 or 2 parameters, but has {callbackParameters.Count}");
            case 2:
                if (!callbackParameters[1].types.Contains(ResultType.Number))
                    throw new InvalidArgumentTypeException(null, $"Second parameter of callback function must accept numbers, but is {string.Join("|", callbackParameters[1].types.Select(t => t.ToTypeName()))}");

                argsConverter = (element, index) =>
                {
                    var elementExpression = element.ToExpression();
                    var indexExpression = ConstantExpression.Number(index);

                    return new[] { elementExpression, indexExpression };
                };

                break;
            default:
                argsConverter = (element, _) =>
                {
                    return new[] { element.ToExpression() };
                };

                break;
        }

        return list.Select(argsConverter).ToAsyncEnumerable();
    }

    protected abstract IAsyncEnumerable<ExpressionResult> EvaluateListManipulationAsync(Interpreter interpreter, IAsyncEnumerable<ConstantExpression[]> arguments, FunctionDefinition callback, CancellationToken cancellationToken = default);
}