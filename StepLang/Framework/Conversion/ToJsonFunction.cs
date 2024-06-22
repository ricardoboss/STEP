using System.Text.Json;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Conversion;

/// <summary>
/// Converts an <see cref="ExpressionResult"/> to a JSON string.
/// </summary>
public class ToJsonFunction : GenericFunction<ExpressionResult>
{
    /// <summary>
    /// The identifier of the <see cref="ToJsonFunction"/>.
    /// </summary>
    public const string Identifier = "toJson";

    /// <inheritdoc />
    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(AnyValueType, "value"),
    };

    /// <inheritdoc />
    protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyString;

    /// <inheritdoc />
    protected override StringResult Invoke(TokenLocation callLocation, Interpreter interpreter, ExpressionResult argument1) => JsonSerializer.Serialize(argument1, JsonConversionContext.Default.ExpressionResult);
}