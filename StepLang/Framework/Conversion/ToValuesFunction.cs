using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Conversion;

/// <summary>
/// Converts a map to a list of its values.
/// </summary>
public class ToValuesFunction : GenericFunction<MapResult>
{
    /// <summary>
    /// The identifier of the <see cref="ToValuesFunction"/> function.
    /// </summary>
    public const string Identifier = "toValues";

    /// <inheritdoc />
    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(OnlyMap, "source"),
    };

    /// <inheritdoc />
    protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyList;

    /// <inheritdoc />
    protected override ExpressionResult Invoke(TokenLocation callLocation, Interpreter interpreter, MapResult argument1)
    {
        var map = argument1.Value;

        return new ListResult(map.Values.ToList());
    }
}