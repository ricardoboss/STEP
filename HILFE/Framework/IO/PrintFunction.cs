using System.Globalization;
using HILFE.Interpreting;
using HILFE.Parsing.Expressions;

namespace HILFE.Framework.IO;

public class PrintFunction : NativeFunction
{
    /// <inheritdoc />
    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        if (interpreter.StdOut is not { } stdOut)
            return ExpressionResult.Void;

        var stringArgs = await arguments
            .EvaluateAsync(interpreter, cancellationToken)
            .Select(RenderValue)
            .ToListAsync(cancellationToken);

        await Print(stdOut, string.Join("", stringArgs), cancellationToken);

        return ExpressionResult.Void;
    }

    private static string RenderValue(ExpressionResult result)
    {
        return result.ValueType switch
        {
            "string" when result.Value is string stringValue => stringValue,
            "number" when result.Value is double numberValue => numberValue.ToString(CultureInfo.InvariantCulture),
            "bool" when result.Value is bool boolValue => boolValue.ToString(),
            "null" when result.Value is null => "null",
            _ => $"[{result.ValueType}]",
        };
    }

    protected virtual async Task Print(TextWriter output, string value, CancellationToken cancellationToken = default)
        => await output.WriteAsync(value.AsMemory(), cancellationToken);

    /// <inheritdoc />
    protected override string DebugParamsString => "string ...args";
}