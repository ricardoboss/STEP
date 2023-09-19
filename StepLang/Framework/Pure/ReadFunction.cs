using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Framework.Pure;

public class ReadFunction : NativeFunction
{
    public const string Identifier = "read";

    /// <inheritdoc />
    public override Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments);

        if (interpreter.StdIn is not { } stdIn)
            return Task.FromResult<ExpressionResult>(StringResult.Empty);

        var character = stdIn.Read();
        if (character < 0)
            return Task.FromResult<ExpressionResult>(StringResult.Empty);

        return Task.FromResult<ExpressionResult>(new StringResult(char.ConvertFromUtf32(character)));
    }
}