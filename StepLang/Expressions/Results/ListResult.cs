namespace StepLang.Expressions.Results;

public class ListResult : ValueExpressionResult<IList<ExpressionResult>>
{
	public static ListResult Empty => new(new List<ExpressionResult>());

	public static ListResult From(params ExpressionResult[] results)
	{
		return new ListResult(results.ToList());
	}

	/// <inheritdoc />
	public ListResult(IList<ExpressionResult> value) : base(ResultType.List, value)
	{
	}

	protected override bool EqualsInternal(ExpressionResult other)
	{
		return other is ListResult listResult && Value.SequenceEqual(listResult.Value);
	}

	public override ListResult DeepClone()
	{
		var clone = Value.Select(result => result.DeepClone()).ToList();

		return new ListResult(clone);
	}

	/// <inheritdoc />
	public override string ToString()
	{
		return $"[{string.Join(", ", Value)}]";
	}
}
