using HILFE.Interpreting;
using HILFE.Parsing.Expressions;

namespace HILFE.Framework.Functions;

public class PrintlnFunction : FunctionDefinition
{
    /// <inheritdoc />
    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        if (interpreter.StdOut is not { } stdOut)
            return new("void", IsVoid: true);

        var stringArgs = await arguments
            .EvaluateAsync(interpreter, cancellationToken)
            .Select(r => r.Value?.ToString() ?? string.Empty)
            .Cast<string>()
            .ToListAsync(cancellationToken);

        await stdOut.WriteLineAsync(string.Join("", stringArgs));

        return new("void", IsVoid: true);
    }

    /// <inheritdoc />
    protected override string DebugBodyString => "[native code]";

    /// <inheritdoc />
    protected override string DebugParamsString => "string ...args";
}