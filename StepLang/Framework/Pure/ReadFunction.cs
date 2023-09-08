using System.Diagnostics.CodeAnalysis;
using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework.Pure;

public class ReadFunction : NativeFunction
{
    public const string Identifier = "read";

    /// <inheritdoc />
    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments, 0);

        if (interpreter.StdIn is not { } stdIn)
            return StringResult.Empty;

        var character = stdIn.Read();
        if (character < 0)
            return StringResult.Empty;

        return new StringResult(char.ConvertFromUtf32(character));
    }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    protected override string DebugParamsString => string.Empty;
}