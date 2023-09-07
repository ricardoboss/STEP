namespace StepLang.Parsing.Expressions;

public abstract class ValueExpressionResult<T> : ExpressionResult, IEquatable<ValueExpressionResult<T>>
    where T : notnull
{
    protected ValueExpressionResult(ResultType resultType, T value) : base(resultType)
    {
        Value = value;
    }

    public T Value { get; }

    public override bool Equals(object? obj)
    {
        return obj is ValueExpressionResult<T> other && Equals(other);
    }

    public bool Equals(ValueExpressionResult<T>? other)
    {
        if (other is null)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (ResultType != other.ResultType)
            return false;

        var comparer = EqualityComparer<T>.Default;

        return comparer.Equals(Value, other.Value);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(ResultType, Value);
    }
}