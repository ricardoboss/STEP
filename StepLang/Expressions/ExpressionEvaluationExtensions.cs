using System.Runtime.CompilerServices;
using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Expressions;

public static class ExpressionEvaluationExtensions
{
    public static async IAsyncEnumerable<ExpressionResult> EvaluateAsync(this IEnumerable<Expression> expressions, Interpreter interpreter, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        foreach (var expression in expressions)
        {
            yield return await expression.EvaluateAsync(interpreter, cancellationToken);
        }
    }

    public static async IAsyncEnumerable<KeyValuePair<string, ExpressionResult>> EvaluateAsync(this IEnumerable<KeyValuePair<string, Expression>> expressions, Interpreter interpreter, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        foreach (var (key, expression) in expressions)
        {
            var value = await expression.EvaluateAsync(interpreter, cancellationToken);

            yield return new(key, value);
        }
    }
}