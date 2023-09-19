using System.Text.Json;
using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Framework.Conversion;

public class ToJsonFunction : NativeFunction
{
    public const string Identifier = "toJson";

    public override IEnumerable<(ResultType[] types, string identifier)> Parameters => new[] { (Enum.GetValues<ResultType>(), "value") };

    /// <inheritdoc />
    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        var exp = arguments.Single();
        var result = await exp.EvaluateAsync(interpreter, cancellationToken);
        var json = JsonSerializer.Serialize(result, JsonConversionContext.Default.ExpressionResult);
        return new StringResult(json);
    }
}