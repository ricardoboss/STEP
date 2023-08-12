using System.Diagnostics.CodeAnalysis;
using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework.IO;

public class ReadlineFunction : NativeFunction
{
    public override string Identifier { get; } =  "readline";

    /// <inheritdoc />
    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        if (arguments.Count is not 0)
            throw new InvalidArgumentCountException(0, arguments.Count);

        if (interpreter.StdIn is not { } stdIn)
            return StringResult.Empty;

        var line = await stdIn.ReadLineAsync(cancellationToken) ?? string.Empty;

        return new StringResult(line);
    }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    protected override string DebugParamsString => string.Empty;
}