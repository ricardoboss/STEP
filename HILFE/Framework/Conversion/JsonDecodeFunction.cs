using System.Text.Json;
using HILFE.Interpreting;
using HILFE.Parsing.Expressions;

namespace HILFE.Framework.Conversion;

public class JsonDecodeFunction : NativeFunction
{
    public const string Identifier = "jsonDecode";

    /// <inheritdoc />
    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        var source = arguments.Single();
        var result = await source.EvaluateAsync(interpreter, cancellationToken);
        var json = result.ExpectString();

        var value = JsonSerializer.Deserialize<dynamic>(json);
        var valueType = value switch
        {
            string => "string",
            int or double or float => "number",
            bool => "bool",
            null => "null",
            List<dynamic> => "array",
            _ => "object",
        };

        return new(valueType, value);
    }

    /// <inheritdoc />
    protected override string DebugParamsString => "string json";
}