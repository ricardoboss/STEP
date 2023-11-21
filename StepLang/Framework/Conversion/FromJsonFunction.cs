using System.Text.Json;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Conversion;

/// <summary>
/// Converts a JSON string to an <see cref="ExpressionResult"/>.
/// </summary>
public class FromJsonFunction : GenericFunction<StringResult>
{
    /// <summary>
    /// The identifier of the <see cref="FromJsonFunction"/>.
    /// </summary>
    public const string Identifier = "fromJson";

    /// <inheritdoc />
    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(OnlyString, "source"),
    };

    /// <inheritdoc />
    protected override IEnumerable<ResultType> ReturnTypes { get; } = AnyValueType;

    /// <inheritdoc />
    protected override ExpressionResult Invoke(TokenLocation callLocation, Interpreter interpreter,
        StringResult argument1)
    {
        try
        {
            return JsonSerializer.Deserialize(argument1.Value, JsonConversionContext.Default.ExpressionResult) ??
                   NullResult.Instance;
        }
        catch (JsonException)
        {
            return NullResult.Instance;
        }
    }
}