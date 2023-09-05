using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework.Pure;

public class ConvertedFunction : NativeFunction
{
    public const string Identifier = "converted";

    public override IEnumerable<(ResultType[] types, string identifier)> Parameters => new[] { (new[] { ResultType.List }, "subject"), (new[] { ResultType.Function }, "callback") };

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments, 2);

        var (subjectExpression, callbackExpression) = (arguments[0], arguments[1]);

        var subjectResult = await subjectExpression.EvaluateAsync(interpreter, r => r.ExpectList(), cancellationToken);
        var callback = await callbackExpression.EvaluateAsync(interpreter, r => r.ExpectFunction().Value, cancellationToken);

        var callbackParameters = callback.Parameters.ToList();
        switch (callbackParameters.Count)
        {
            case < 1 or > 2:
                throw new InvalidArgumentTypeException(null, $"Callback function must have 1 or 2 parameters, but has {callbackParameters.Count}");
            case 2 when !callbackParameters[1].types.Contains(ResultType.Number):
                throw new InvalidArgumentTypeException(null, $"Second parameter of callback function must accept numbers, but is {string.Join("|", callbackParameters[1].types.Select(t => t.ToTypeName()))}");
        }

        Func<ExpressionResult, int, ConstantExpression[]> argsConverter = callbackParameters.Count switch
        {
            2 => (element, index) =>
            {
                var elementExpression = new ConstantExpression(element);
                var indexExpression = ConstantExpression.Number(index);

                return new[] { elementExpression, indexExpression };
            }
            ,
            _ => (element, _) =>
            {
                var elementExpression = new ConstantExpression(element);

                return new[] { elementExpression };
            }
            ,
        };

        var converted = await subjectResult
            .DeepClone()
            .Value
            .Select(argsConverter)
            .ToAsyncEnumerable()
            .SelectAwait(async args => await callback.EvaluateAsync(interpreter, args, cancellationToken))
            .ToListAsync(cancellationToken);

        return new ListResult(converted);
    }
}