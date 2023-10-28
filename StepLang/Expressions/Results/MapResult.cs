using System.Diagnostics.CodeAnalysis;

namespace StepLang.Expressions.Results;

/// <summary>
/// Represents a map of evaluated expressions (<see cref="ExpressionResult"/>) accessible via a string key.
/// </summary>
public class MapResult : ValueExpressionResult<Dictionary<string, ExpressionResult>>
{
    /// <summary>
    /// Returns an empty map.
    /// </summary>
    public static MapResult Empty => new(new());

    /// <summary>
    /// Creates a new <see cref="MapResult"/> with the given string keys and <see cref="ExpressionResult"/>s.
    /// </summary>
    /// <param name="value">The map of string keys and <see cref="ExpressionResult"/>s to use.</param>
    public MapResult(Dictionary<string, ExpressionResult> value) : base(ResultType.Map, value)
    {
    }

    /// <inheritdoc />
    protected override bool EqualsInternal(ExpressionResult other)
    {
        return other is MapResult mapResult && Value.SequenceEqual(mapResult.Value);
    }

    /// <inheritdoc />
    public override MapResult DeepClone()
    {
        var clone = Value.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.DeepClone());

        return new(clone);
    }

    /// <inheritdoc />
    public override string ToString() => $"{{{string.Join(", ", Value.Select(p => $"{p.Key}: {p.Value}"))}}}";

    public static implicit operator MapResult(Dictionary<string, ExpressionResult> value) => new(value);

    [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "To prevent CA2225")]
    public static MapResult FromDictionary(Dictionary<string, ExpressionResult> value) => value;
}
