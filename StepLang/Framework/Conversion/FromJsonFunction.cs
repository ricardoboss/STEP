using System.Text.Json;
using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Framework.Conversion;

public class FromJsonFunction : NativeFunction
{
    public const string Identifier = "fromJson";

    public override IEnumerable<(ResultType[] types, string identifier)> Parameters => new[] { (new[] { ResultType.Str }, "source") };

    /// <inheritdoc />
    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments, CancellationToken cancellationToken = default)
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
}