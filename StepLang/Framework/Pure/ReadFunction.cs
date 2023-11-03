using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;

namespace StepLang.Framework.Pure;

public class ReadFunction : NativeFunction
{
    public const string Identifier = "read";

    /// <inheritdoc />
    protected override Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments);

        if (interpreter.StdIn is not { } stdIn)
            return Task.FromResult<ExpressionResult>(NullResult.Instance);

        var character = stdIn.Read();
        if (character < 0)
            return Task.FromResult<ExpressionResult>(NullResult.Instance);

        return Task.FromResult<ExpressionResult>(new StringResult(char.ConvertFromUtf32(character)));
    }
}