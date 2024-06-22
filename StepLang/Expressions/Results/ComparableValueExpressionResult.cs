namespace StepLang.Expressions.Results;

/// <summary>
/// Represents an expression result that holds a comparable value.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class ComparableValueExpressionResult<T> : ValueExpressionResult<T>, IComparable<ComparableValueExpressionResult<T>>, IComparable where T : notnull
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ComparableValueExpressionResult{T}"/> class.
    /// </summary>
    /// <param name="resultType">The type of the result. See <see cref="ResultType"/></param>
    /// <param name="value">The value of the result.</param>
    protected ComparableValueExpressionResult(ResultType resultType, T value) : base(resultType, value)
    {
    }

    /// <summary>
    /// Compares this <see cref="ComparableValueExpressionResult{T}"/> to another <see cref="ComparableValueExpressionResult{T}"/>.
    /// </summary>
    /// <param name="other">The other <see cref="ComparableValueExpressionResult{T}"/> to compare to.</param>
    /// <returns>
    /// <para>
    /// An integer indicating the relative order of the objects being compared.
    /// </para>
    /// <list type="bullet">
    /// <listheader><description>Return Value:</description></listheader>
    /// <item><description>A value less than zero indicates that this instance is less than <paramref name="other"/>.</description></item>
    /// <item><description>A value of zero indicates that this instance is equal to <paramref name="other"/>.</description></item>
    /// <item><description>A value greater than zero indicates that this instance is greater than <paramref name="other"/>.</description></item>
    /// </list>
    /// </returns>
    public int CompareTo(ComparableValueExpressionResult<T>? other)
    {
        if (ReferenceEquals(null, other))
            return 1;

        return CompareToInternal(other);
    }

    /// <summary>
    /// Compares this <see cref="ComparableValueExpressionResult{T}"/> to another <see cref="ComparableValueExpressionResult{T}"/> if this instance is not the same as <paramref name="other"/>.
    /// </summary>
    /// <param name="other">The other <see cref="ComparableValueExpressionResult{T}"/> to compare to.</param>
    /// <returns>A value less than zero if this <see cref="ComparableValueExpressionResult{T}"/> is less than <paramref name="other"/>.</returns>
    protected abstract int CompareToInternal(ComparableValueExpressionResult<T> other);

    /// <inheritdoc />
    public int CompareTo(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return 1;

        if (ReferenceEquals(this, obj))
            return 0;

        return obj is ComparableValueExpressionResult<T> other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(ComparableValueExpressionResult<T>)}");
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) => base.Equals(obj);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(ResultType, Value);

    /// <summary>
    /// Compares two <see cref="ComparableValueExpressionResult{T}"/>s for equality.
    /// </summary>
    /// <param name="left">The left <see cref="ComparableValueExpressionResult{T}"/>.</param>
    /// <param name="right">The right <see cref="ComparableValueExpressionResult{T}"/>.</param>
    /// <returns>
    /// <c>true</c> if the two <see cref="ComparableValueExpressionResult{T}"/>s are equal; otherwise, <c>false</c>.
    /// </returns>
    public static bool operator ==(ComparableValueExpressionResult<T> left, ComparableValueExpressionResult<T> right) => left.Equals(right);

    /// <summary>
    /// Compares two <see cref="ComparableValueExpressionResult{T}"/>s for inequality.
    /// Uses <see cref="operator ==(ComparableValueExpressionResult{T}, ComparableValueExpressionResult{T})"/> and negates the result.
    /// </summary>
    /// <param name="left">The left <see cref="ComparableValueExpressionResult{T}"/>.</param>
    /// <param name="right">The right <see cref="ComparableValueExpressionResult{T}"/>.</param>
    /// <returns>
    /// <c>true</c> if the two <see cref="ComparableValueExpressionResult{T}"/>s are not equal; otherwise, <c>false</c>.
    /// </returns>
    public static bool operator !=(ComparableValueExpressionResult<T> left, ComparableValueExpressionResult<T> right) => !(left == right);

    /// <summary>
    /// Compares two <see cref="ComparableValueExpressionResult{T}"/>s for less than using <see cref="CompareTo(ComparableValueExpressionResult{T})"/>.
    /// </summary>
    /// <param name="left">The left <see cref="ComparableValueExpressionResult{T}"/>.</param>
    /// <param name="right">The right <see cref="ComparableValueExpressionResult{T}"/>.</param>
    /// <returns>
    /// <c>true</c> if the left <see cref="ComparableValueExpressionResult{T}"/> is less than the right <see cref="ComparableValueExpressionResult{T}"/>; otherwise, <c>false</c>.
    /// </returns>
    public static bool operator <(ComparableValueExpressionResult<T> left, ComparableValueExpressionResult<T> right)
    {
        if (ReferenceEquals(left, null))
            return !ReferenceEquals(right, null);

        return left.CompareTo(right) < 0;
    }

    /// <summary>
    /// Compares two <see cref="ComparableValueExpressionResult{T}"/>s for less than or equal using <see cref="CompareTo(ComparableValueExpressionResult{T})"/>.
    /// </summary>
    /// <param name="left">The left <see cref="ComparableValueExpressionResult{T}"/>.</param>
    /// <param name="right">The right <see cref="ComparableValueExpressionResult{T}"/>.</param>
    /// <returns>
    /// <c>true</c> if the left <see cref="ComparableValueExpressionResult{T}"/> is less than or equal to the right <see cref="ComparableValueExpressionResult{T}"/>; otherwise, <c>false</c>.
    /// </returns>
    public static bool operator <=(ComparableValueExpressionResult<T> left, ComparableValueExpressionResult<T> right)
    {
        return ReferenceEquals(left, null) || left.CompareTo(right) <= 0;
    }

    /// <summary>
    /// Compares two <see cref="ComparableValueExpressionResult{T}"/>s for greater than using <see cref="CompareTo(ComparableValueExpressionResult{T})"/>.
    /// </summary>
    /// <param name="left">The left <see cref="ComparableValueExpressionResult{T}"/>.</param>
    /// <param name="right">The right <see cref="ComparableValueExpressionResult{T}"/>.</param>
    /// <returns>
    /// <c>true</c> if the left <see cref="ComparableValueExpressionResult{T}"/> is greater than the right <see cref="ComparableValueExpressionResult{T}"/>; otherwise, <c>false</c>.
    /// </returns>
    public static bool operator >(ComparableValueExpressionResult<T> left, ComparableValueExpressionResult<T> right)
    {
        return !ReferenceEquals(left, null) && left.CompareTo(right) > 0;
    }

    /// <summary>
    /// Compares two <see cref="ComparableValueExpressionResult{T}"/>s for greater than or equal using <see cref="CompareTo(ComparableValueExpressionResult{T})"/>.
    /// </summary>
    /// <param name="left">The left <see cref="ComparableValueExpressionResult{T}"/>.</param>
    /// <param name="right">The right <see cref="ComparableValueExpressionResult{T}"/>.</param>
    /// <returns>
    /// <c>true</c> if the left <see cref="ComparableValueExpressionResult{T}"/> is greater than or equal to the right <see cref="ComparableValueExpressionResult{T}"/>; otherwise, <c>false</c>.
    /// </returns>
    public static bool operator >=(ComparableValueExpressionResult<T> left, ComparableValueExpressionResult<T> right)
    {
        if (ReferenceEquals(left, null))
            return ReferenceEquals(right, null);

        return left.CompareTo(right) >= 0;
    }
}