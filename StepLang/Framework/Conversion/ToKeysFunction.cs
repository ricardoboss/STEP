using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Conversion;

/// <summary>
/// Converts a map to a list of its keys.
/// </summary>
public class ToKeysFunction : GenericFunction<MapResult>
{
    /// <summary>
    /// The identifier of the <see cref="ToKeysFunction"/> function.
    /// </summary>
    public const string Identifier = "toKeys";

    /// <inheritdoc />
    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(OnlyMap, "source"),
    };

    /// <inheritdoc />
    protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyList;

    /// <inheritdoc />
    protected override ListResult Invoke(TokenLocation callLocation, Interpreter interpreter, MapResult argument1)
    {
        var keys = argument1.Value.Keys
            .Select(k => new StringResult(k))
            .Cast<ExpressionResult>()
            .ToList();

        return new(keys);
    }
}