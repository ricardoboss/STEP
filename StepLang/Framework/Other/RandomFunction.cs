using System.Diagnostics.CodeAnalysis;
using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

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