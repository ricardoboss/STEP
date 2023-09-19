using System.Diagnostics.CodeAnalysis;
using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Framework.Other;

[SuppressMessage("Security", "CA5394:Do not use insecure randomness")]
public class RandomFunction : NativeFunction
{
    public const string Identifier = "random";

    public override Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments);

        var value = interpreter.Random.NextDouble();

        return Task.FromResult<ExpressionResult>(new NumberResult(value));
    }
}