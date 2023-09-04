namespace StepLang.Parsing.Expressions;

public abstract class ComparableValueExpressionResult<T> : ValueExpressionResult<T>, IComparable<ComparableValueExpressionResult<T>>, IComparable
{
    protected ComparableValueExpressionResult(ResultType resultType, T value) : base(resultType, value)
    {
    }

    public int CompareTo(ComparableValueExpressionResult<T>? other)
    {
        if (ReferenceEquals(null, other))
            return 1;

        return CompareToInternal(other);
    }

    protected abstract int CompareToInternal(ComparableValueExpressionResult<T> other);

    public int CompareTo(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return 1;

        if (ReferenceEquals(this, obj))
            return 0;

        return obj is ComparableValueExpressionResult<T> other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(ComparableValueExpressionResult<T>)}");
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
            return true;

        if (ReferenceEquals(obj, null))
            return false;

        if (obj is not ComparableValueExpressionResult<T> other)
            return false;

        return CompareTo(other) == 0;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(ResultType, Value);
    }

    public static bool operator ==(ComparableValueExpressionResult<T> left, ComparableValueExpressionResult<T> right)
    {
        if (ReferenceEquals(left, null))
        {
            return ReferenceEquals(right, null);
        }

        return left.Equals(right);
    }

    public static bool operator !=(ComparableValueExpressionResult<T> left, ComparableValueExpressionResult<T> right)
    {
        return !(left == right);
    }

    public static bool operator <(ComparableValueExpressionResult<T> left, ComparableValueExpressionResult<T> right)
    {
        return ReferenceEquals(left, null) ? !ReferenceEquals(right, null) : left.CompareTo(right) < 0;
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
        return ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.CompareTo(right) >= 0;
    }
}