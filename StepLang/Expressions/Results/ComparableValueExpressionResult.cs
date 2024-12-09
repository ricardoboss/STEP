namespace StepLang.Expressions.Results;

public abstract class ComparableValueExpressionResult<T>(ResultType resultType, T value)
	: ValueExpressionResult<T>(resultType, value),
		IComparable<ComparableValueExpressionResult<T>>, IComparable
	where T : notnull
{
	public int CompareTo(ComparableValueExpressionResult<T>? other)
	{
		if (ReferenceEquals(null, other))
		{
			return 1;
		}

		return CompareToInternal(other);
	}

	protected abstract int CompareToInternal(ComparableValueExpressionResult<T> other);

	public int CompareTo(object? obj)
	{
		if (ReferenceEquals(null, obj))
		{
			return 1;
		}

		if (ReferenceEquals(this, obj))
		{
			return 0;
		}

		return obj is ComparableValueExpressionResult<T> other ?
			CompareTo(other) :
			throw new ArgumentException($"Object must be of type {nameof(ComparableValueExpressionResult<T>)}");
	}

	public override bool Equals(object? obj)
	{
		return base.Equals(obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(ResultType, Value);
	}

	public static bool operator ==(ComparableValueExpressionResult<T> left, ComparableValueExpressionResult<T> right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(ComparableValueExpressionResult<T> left, ComparableValueExpressionResult<T> right)
	{
		return !(left == right);
	}

	public static bool operator <(ComparableValueExpressionResult<T> left, ComparableValueExpressionResult<T> right)
	{
		if (ReferenceEquals(left, null))
		{
			return !ReferenceEquals(right, null);
		}

		return left.CompareTo(right) < 0;
	}

	public static bool operator <=(ComparableValueExpressionResult<T> left, ComparableValueExpressionResult<T> right)
	{
		return ReferenceEquals(left, null) || left.CompareTo(right) <= 0;
	}

	public static bool operator >(ComparableValueExpressionResult<T> left, ComparableValueExpressionResult<T> right)
	{
		return !ReferenceEquals(left, null) && left.CompareTo(right) > 0;
	}

	public static bool operator >=(ComparableValueExpressionResult<T> left, ComparableValueExpressionResult<T> right)
	{
		if (ReferenceEquals(left, null))
		{
			return ReferenceEquals(right, null);
		}

		return left.CompareTo(right) >= 0;
	}
}
