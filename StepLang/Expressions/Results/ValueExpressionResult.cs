namespace StepLang.Expressions.Results;

public abstract class ValueExpressionResult<T>(ResultType resultType, T value) : ExpressionResult(resultType)
	where T : notnull
{
	public T Value { get; } = value;

	public override bool Equals(object? obj)
	{
		return obj is ValueExpressionResult<T> other && Equals(other);
	}

	/// <inheritdoc />
	public override int GetHashCode()
	{
		return HashCode.Combine(ResultType, Value);
	}
}
