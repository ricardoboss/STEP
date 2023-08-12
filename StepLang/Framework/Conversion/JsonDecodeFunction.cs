using System.Text.Json;
using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework.Conversion;

public class JsonDecodeFunction : NativeFunction
{
    public override string Identifier { get; } = "jsonDecode";

    /// <inheritdoc />
    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        var source = arguments.Single();
        var result = await source.EvaluateAsync(interpreter, cancellationToken);
        var json = result.ExpectString().Value;
        return JsonSerializer.Deserialize(json, JsonConversionContext.Default.ExpressionResult) ?? NullResult.Instance;
    }

    /// <inheritdoc />
    protected override string DebugParamsString => $"{ResultType.Str.ToTypeName()} json";
}