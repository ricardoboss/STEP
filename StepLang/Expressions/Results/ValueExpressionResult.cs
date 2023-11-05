namespace StepLang.Expressions.Results;

public abstract class ValueExpressionResult<T> : ExpressionResult
    where T : notnull
{
    protected ValueExpressionResult(ResultType resultType, T value) : base(resultType) => Value = value;

    public T Value { get; }

    public override bool Equals(object? obj) => obj is ValueExpressionResult<T> other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(ResultType, Value);
}