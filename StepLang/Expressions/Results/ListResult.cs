namespace StepLang.Expressions.Results;

/// <summary>
/// Represents a list of evaluated expressions (<see cref="ExpressionResult"/>) ordered by an integer index.
/// </summary>
public class ListResult : ValueExpressionResult<IList<ExpressionResult>>
{
    /// <summary>
    /// Returns an empty list.
    /// </summary>
    public static ListResult Empty => new(new List<ExpressionResult>());

    /// <summary>
    /// Creates a new <see cref="ListResult"/> with the given <see cref="ExpressionResult"/>s.
    /// </summary>
    /// <param name="value">The <see cref="ExpressionResult"/>s to use.</param>
    public ListResult(IList<ExpressionResult> value) : base(ResultType.List, value)
    {
    }

    /// <inheritdoc />
    protected override bool EqualsInternal(ExpressionResult other)
    {
        return other is ListResult listResult && Value.SequenceEqual(listResult.Value);
    }

    /// <inheritdoc />
    public override ListResult DeepClone()
    {
        var clone = Value.Select(result => result.DeepClone()).ToList();

        return new(clone);
    }

    /// <inheritdoc />
    public override string ToString() => $"[{string.Join(", ", Value)}]";
}