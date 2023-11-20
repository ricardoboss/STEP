using System.Text;

namespace StepLang.Expressions.Results;

/// <summary>
/// Represents a <see cref="string"/> result.
/// </summary>
public class StringResult : ComparableValueExpressionResult<string>
{
    /// <summary>
    /// A new <see cref="StringResult"/> with an empty <see cref="string"/>.
    /// </summary>
    public static StringResult Empty => new(string.Empty);

    /// <summary>
    /// Creates a new <see cref="StringResult"/> with the given value.
    /// </summary>
    /// <param name="value">The value of the result.</param>
    public StringResult(string value) : base(ResultType.Str, value)
    {
    }

    /// <inheritdoc />
    protected override int CompareToInternal(ComparableValueExpressionResult<string> other)
    {
        var normalizedA = Value.Normalize(NormalizationForm.FormD);
        var normalizedB = other.Value.Normalize(NormalizationForm.FormD);

        return string.Compare(normalizedA, normalizedB, StringComparison.Ordinal);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is ExpressionResult result && EqualsInternal(result);

    /// <inheritdoc />
    protected override bool EqualsInternal(ExpressionResult other) => other is StringResult stringResult && string.Equals(Value, stringResult.Value, StringComparison.Ordinal);

    /// <inheritdoc />
    public override int GetHashCode() => base.GetHashCode();

    /// <inheritdoc />
    public override StringResult DeepClone()
    {
        return new(Value);
    }

    /// <inheritdoc />
    public override string ToString() => $"\"{Value}\"";

    /// <summary>
    /// Implicitly converts a <see cref="string"/> to a <see cref="StringResult"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The converted value.</returns>
    public static implicit operator StringResult(string value) => new(value);

    /// <summary>
    /// Concatenates two <see cref="StringResult"/>s to a new <see cref="StringResult"/>.
    /// </summary>
    /// <param name="left">The first <see cref="StringResult"/>.</param>
    /// <param name="right">The second <see cref="StringResult"/>.</param>
    /// <returns>The concatenated <see cref="StringResult"/>.</returns>
    public static StringResult operator +(StringResult left, StringResult right) => new(left.Value + right.Value);

    /// <summary>
    /// Concatenates a <see cref="StringResult"/> and a <see cref="NumberResult"/> to a new <see cref="StringResult"/>.
    /// </summary>
    /// <param name="left">The <see cref="StringResult"/>.</param>
    /// <param name="right">The <see cref="NumberResult"/>.</param>
    /// <returns>The concatenated <see cref="StringResult"/>.</returns>
    public static StringResult operator +(StringResult left, NumberResult right) => new(left.Value + right.Value);

    /// <summary>
    /// Concatenates a <see cref="NumberResult"/> and a <see cref="StringResult"/> to a new <see cref="StringResult"/>.
    /// </summary>
    /// <param name="left">The <see cref="NumberResult"/>.</param>
    /// <param name="right">The <see cref="StringResult"/>.</param>
    /// <returns>The concatenated <see cref="StringResult"/>.</returns>
    public static StringResult operator +(NumberResult left, StringResult right) => new(left.Value + right.Value);

    /// <summary>
    /// Checks if two <see cref="StringResult"/>s are equal using <see cref="string.Equals(string, string, StringComparison)"/> with <see cref="StringComparison.Ordinal"/>.
    /// </summary>
    /// <param name="left">The first <see cref="StringResult"/>.</param>
    /// <param name="right">The second <see cref="StringResult"/>.</param>
    /// <returns>A <see cref="BoolResult"/> with the result of the comparison.</returns>
    public static BoolResult operator ==(StringResult left, StringResult right) => new(string.Equals(left.Value, right.Value, StringComparison.Ordinal));

    /// <summary>
    /// Checks if two <see cref="StringResult"/>s are not equal using <see cref="string.Equals(string, string, StringComparison)"/> with <see cref="StringComparison.Ordinal"/>.
    /// </summary>
    /// <param name="left">The first <see cref="StringResult"/>.</param>
    /// <param name="right">The second <see cref="StringResult"/>.</param>
    /// <returns>A <see cref="BoolResult"/> with the result of the comparison.</returns>
    public static BoolResult operator !=(StringResult left, StringResult right) => new(!string.Equals(left.Value, right.Value, StringComparison.Ordinal));
}