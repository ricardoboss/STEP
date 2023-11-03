using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;

namespace StepLang.Framework.Pure;

public class ReadlineFunction : NativeFunction
{
    public const string Identifier = "readline";

    /// <inheritdoc />
    protected override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments);

        if (interpreter.StdIn is not { } stdIn)
            return NullResult.Instance;

        var line = await stdIn.ReadLineAsync(cancellationToken) ?? string.Empty;

        return new StringResult(line);
    }
}