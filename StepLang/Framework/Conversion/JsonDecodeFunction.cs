using System.Text.Json;
using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework.Conversion;

public class JsonDecodeFunction : NativeFunction
{
    public const string Identifier = "jsonDecode";

    /// <inheritdoc />
    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        var source = arguments.Single();
        var result = await source.EvaluateAsync(interpreter, cancellationToken);
        var json = result.ExpectString().Value;
        return JsonSerializer.Deserialize<ExpressionResult>(json, new JsonSerializerOptions
        {
            Converters =
            {
                new ExpressionResultJsonConverter(),
            },
        }) ?? NullResult.Instance;
    }

    /// <inheritdoc />
    protected override string DebugParamsString => $"{ResultType.Str.ToTypeName()} json";
}