using System.Runtime.CompilerServices;
using HILFE.Interpreting;

namespace HILFE.Parsing.Expressions;

public static class IReadOnlyListExpressionExtensions
{
    public static async IAsyncEnumerable<ExpressionResult> EvaluateAsync(this IEnumerable<Expression> expressions, Interpreter interpreter, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        foreach (var expression in expressions)
        {
            yield return await expression.EvaluateAsync(interpreter, cancellationToken);
        }
    }
}