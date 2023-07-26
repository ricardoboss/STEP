using System.Diagnostics.CodeAnalysis;
using HILFE.Interpreting;
using HILFE.Parsing.Expressions;

namespace HILFE.Framework.IO;

public class ReadlineFunction : NativeFunction
{
    public const string Identifier = "readline";

    /// <inheritdoc />
    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        if (arguments.Count is not 0)
            throw new InvalidArgumentCountException(0, arguments.Count);

        if (interpreter.StdIn is not { } stdIn)
            return ExpressionResult.Null;

        var line = await stdIn.ReadLineAsync(cancellationToken);

        return line is null ? ExpressionResult.Null : ExpressionResult.String(line);
    }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    protected override string DebugParamsString => string.Empty;
}