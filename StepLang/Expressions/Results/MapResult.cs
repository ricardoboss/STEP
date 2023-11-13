namespace StepLang.Expressions.Results;

public class MapResult : ValueExpressionResult<IDictionary<string, ExpressionResult>>
{
    public static MapResult Empty => new(new Dictionary<string, ExpressionResult>());

    /// <inheritdoc />
    public MapResult(IDictionary<string, ExpressionResult> value) : base(ResultType.Map, value)
    {
    }

    protected override bool EqualsInternal(ExpressionResult other)
    {
        return other is MapResult mapResult && Value.SequenceEqual(mapResult.Value);
    }

    public override MapResult DeepClone()
    {
        var clone = Value.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.DeepClone());

        return new(clone);
    }

    /// <inheritdoc />
    public override string ToString() => $"{{{string.Join(", ", Value.Select(p => $"{p.Key}: {p.Value}"))}}}";

    public static implicit operator MapResult(Dictionary<string, ExpressionResult> value) => new(value);
}