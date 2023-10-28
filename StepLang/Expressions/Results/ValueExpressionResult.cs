namespace StepLang.Expressions.Results;
/// <summary>
/// <para>
/// Represents a result that has a value.
/// </para>
/// <para>
/// <see cref="ExpressionResult"/>s do not necessarily have a value (e.g. <see cref="VoidResult"/>).
/// Results that do have a value extend this class.
/// </para>
/// </summary>
/// <typeparam name="T">The encapsulated value type. Must not be <see langword="null"/>.</typeparam>

public abstract class ValueExpressionResult<T> : ExpressionResult
    where T : notnull
{
    /// <summary>
    /// Creates a new <see cref="ValueExpressionResult{T}"/> with the given <see cref="ResultType"/> and <paramref name="value"/>.
    /// </summary>
    /// <param name="resultType">The <see cref="ResultType"/> of the result.</param>
    /// <param name="value">The value of the result.</param>
    protected ValueExpressionResult(ResultType resultType, T value) : base(resultType) => Value = value;

    /// <summary>
    /// The value of the result.
    /// </summary>
    public T Value { get; }

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is ValueExpressionResult<T> other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(ResultType, Value);
}