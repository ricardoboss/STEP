using System.Text.Json;
using HILFE.Interpreting;
using HILFE.Parsing.Expressions;

namespace HILFE.Framework.Conversion;

public class JsonEncodeFunction : NativeFunction
{
    public const string Identifier = "jsonEncode";

    /// <inheritdoc />
    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        var exp = arguments.Single();
        var result = await exp.EvaluateAsync(interpreter, cancellationToken);
        var json = JsonSerializer.Serialize(result.Value, new JsonSerializerOptions
        {
            WriteIndented = true,
        });
        return new("string", json);
    }

    /// <inheritdoc />
    protected override string DebugParamsString => "any value";
}