using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;

namespace StepLang.Framework.Pure;

public abstract class ListManipulationFunction : NativeFunction
{
    protected override IEnumerable<NativeParameter> NativeParameters => new NativeParameter[] { (new[] { ResultType.List }, "subject"), (new[] { ResultType.Function }, "callback") };

    protected override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments, CancellationToken cancellationToken = default)
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

    protected virtual IAsyncEnumerable<LiteralExpression[]> PrepareArgsForCallback(IEnumerable<ExpressionResult> list, FunctionDefinition callback)
    {
        var callbackParameters = callback.Parameters.ToList();
        Func<ExpressionResult, int, LiteralExpression[]> argsConverter;

        switch (callbackParameters.Count)
        {
            case < 1 or > 2:
                throw new InvalidArgumentTypeException(null, $"Callback function must have 1 or 2 parameters, but has {callbackParameters.Count}");
            case 2:
                if (!callbackParameters[1].types.Contains(ResultType.Number))
                    throw new InvalidArgumentTypeException(null, $"Second parameter of callback function must accept numbers, but is {string.Join("|", callbackParameters[1].types.Select(t => t.ToTypeName()))}");

                argsConverter = (element, index) =>
                {
                    var elementExpression = element.ToLiteralExpression();
                    var indexExpression = LiteralExpression.Number(index);

                    return new[] { elementExpression, indexExpression };
                };

                break;
            default:
                argsConverter = (element, _) =>
                {
                    return new[] { element.ToLiteralExpression() };
                };

                break;
        }

        return list.Select(argsConverter).ToAsyncEnumerable();
    }

    protected abstract IAsyncEnumerable<ExpressionResult> EvaluateListManipulationAsync(Interpreter interpreter, IAsyncEnumerable<LiteralExpression[]> arguments, FunctionDefinition callback, CancellationToken cancellationToken = default);
}