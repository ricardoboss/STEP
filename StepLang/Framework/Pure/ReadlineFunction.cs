using System.Diagnostics.CodeAnalysis;
using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework.Pure;

public class ReadlineFunction : NativeFunction
{
    public const string Identifier = "readline";

    /// <inheritdoc />
    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments, 0);

        if (interpreter.StdIn is not { } stdIn)
            return StringResult.Empty;

        var line = await stdIn.ReadLineAsync(cancellationToken) ?? string.Empty;

        return new StringResult(line);
    }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    protected override string DebugParamsString => string.Empty;
}