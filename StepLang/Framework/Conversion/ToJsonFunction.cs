using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework.Conversion;

public class ToJsonFunction : NativeFunction
{
    public const string Identifier = "toJson";

    /// <inheritdoc />
    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        var exp = arguments.Single();
        var result = await exp.EvaluateAsync(interpreter, cancellationToken);
        var json = JsonSerializer.Serialize(result, JsonConversionContext.Default.ExpressionResult);
        return new StringResult(json);
    }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    protected override string DebugParamsString => "any value";
}