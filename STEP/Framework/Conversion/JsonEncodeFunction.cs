using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using STEP.Interpreting;
using STEP.Parsing.Expressions;

namespace STEP.Framework.Conversion;

public class JsonEncodeFunction : NativeFunction
{
    public const string Identifier = "jsonEncode";

    /// <inheritdoc />
    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        var exp = arguments.Single();
        var result = await exp.EvaluateAsync(interpreter, cancellationToken);
        var json = JsonSerializer.Serialize(result, new JsonSerializerOptions
        {
            Converters =
            {
                new ExpressionResultJsonConverter(),
            },
        });
        return ExpressionResult.String(json);
    }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    protected override string DebugParamsString => "any value";
}