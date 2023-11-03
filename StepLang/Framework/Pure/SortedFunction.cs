using System.Runtime.CompilerServices;
using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;

namespace StepLang.Framework.Pure;

public class SortedFunction : ListManipulationFunction
{
    public const string Identifier = "sorted";

    protected override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments, 1, 2);

        if (arguments.Count == 2)
            return await base.EvaluateAsync(interpreter, arguments, cancellationToken);

        // use compareTo defined in the current scope and fall back to the default implementation
        ExpressionResult callback;
        if (interpreter.CurrentScope.TryGetVariable(CompareToFunction.Identifier, out var compareToVariable))
            callback = compareToVariable.Value;
        else
            callback = new CompareToFunction().ToResult();

        return await base.EvaluateAsync(interpreter, new[] { arguments[0], callback.ToLiteralExpression() }, cancellationToken);
    }

    protected override IAsyncEnumerable<LiteralExpression[]> PrepareArgsForCallback(IEnumerable<ExpressionResult> list, FunctionDefinition callback)
    {
        var callbackParameters = callback.Parameters.ToList();
        if (callbackParameters.Count != 2)
            throw new InvalidArgumentTypeException(null, $"Callback function must have 2 parameters, but has {callbackParameters.Count}");

        if (!callbackParameters[0].types.SequenceEqual(callbackParameters[1].types))
            throw new InvalidArgumentTypeException(null, $"Both parameters of callback function must have the same type, but are {string.Join("|", callbackParameters[0].types.Select(t => t.ToTypeName()))} and {string.Join("|", callbackParameters[1].types.Select(t => t.ToTypeName()))}");

        return list.Select(e => new[] { e.ToLiteralExpression() }).ToAsyncEnumerable();
    }

    protected override async IAsyncEnumerable<ExpressionResult> EvaluateListManipulationAsync(Interpreter interpreter, IAsyncEnumerable<LiteralExpression[]> arguments,
        FunctionDefinition callback, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var arr = await arguments.ToArrayAsync(cancellationToken);

        Array.Sort(arr, (a, b) =>
        {
            var args = new[] { a[0], b[0] };

            var result = callback.EvaluateAsync(interpreter, args, cancellationToken).Result.ExpectNumber().Value;

            return Math.Sign(result);
        });

        foreach (var arg in arr)
            yield return arg[0].Result;
    }
}