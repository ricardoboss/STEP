namespace StepLang.Parsing.Expressions;

public class ListResult : ValueExpressionResult<IList<ExpressionResult>>
{
    public static ListResult Empty => new(new List<ExpressionResult>());

    public static ListResult From(params ExpressionResult[] results) => new(results.ToList());

    /// <inheritdoc />
    public ListResult(IList<ExpressionResult> value) : base(ResultType.List, value)
    {
    }

    protected override bool EqualsInternal(ExpressionResult other)
    {
        return other is ListResult listResult && Value.SequenceEqual(listResult.Value);
    }
}