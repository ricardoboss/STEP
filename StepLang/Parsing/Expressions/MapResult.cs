namespace StepLang.Parsing.Expressions;

public class MapResult : ValueExpressionResult<IDictionary<string, ExpressionResult>>
{
    public static readonly MapResult Empty = new(new Dictionary<string, ExpressionResult>());

    /// <inheritdoc />
    public MapResult(IDictionary<string, ExpressionResult> value) : base(ResultType.Map, value)
    {
    }

    public override string ToString() => $"{{ {string.Join(", ", Value.Select(kvp => $"{kvp.Key}: {kvp.Value}"))} }}";
}