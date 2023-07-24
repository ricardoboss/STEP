using System.Globalization;
using HILFE.Interpreting;
using HILFE.Parsing.Expressions;

namespace HILFE.Framework.Functions;

public class ParseFunction : FunctionDefinition
{
    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        if (arguments.Count != 2)
            throw new InterpreterException($"Invalid number of arguments, expected 2, got {arguments.Count}");

        var type = await arguments[0].EvaluateAsync(interpreter, cancellationToken);
        var value = await arguments[1].EvaluateAsync(interpreter, cancellationToken);

         if (type.ValueType is not "string")
            throw new InterpreterException($"Invalid type, expected string, got {type.ValueType}");

        if (value.ValueType is not "string")
            throw new InterpreterException($"Invalid value, expected string, got {value.ValueType}");

        var typeString = type.Value as string;
        switch (typeString)
        {
            case "number":
                if (!double.TryParse(value.Value as string, NumberStyles.Any, CultureInfo.InvariantCulture, out var doubleValue))
                    return new("null");

                return new("number", doubleValue);
            case "bool":
                if (!bool.TryParse(value.Value as string, out var boolValue))
                    return new("null");

                return new("bool", boolValue);
            case "string":
                return new("string", value.Value?.ToString() ?? string.Empty);
            default:
                return new("null");
        }
    }

    /// <inheritdoc />
    protected override string DebugBodyString => "[native code]";

    /// <inheritdoc />
    protected override string DebugParamsString => "string type, any value";
}