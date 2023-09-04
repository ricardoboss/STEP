using System.Text.Json;
using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework.Conversion;

public class FromJsonFunction : NativeFunction
{
    public const string Identifier = "fromJson";

    /// <inheritdoc />
    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        var source = arguments.Single();
        var result = await source.EvaluateAsync(interpreter, cancellationToken);
        var json = result.ExpectString().Value;
        try
        {
            return JsonSerializer.Deserialize(json, JsonConversionContext.Default.ExpressionResult) ??
                   NullResult.Instance;
        }
        catch (JsonException)
        {
            return NullResult.Instance;
        }
    }

    /// <inheritdoc />
    protected override string DebugParamsString => $"{ResultType.Str.ToTypeName()} json";
}