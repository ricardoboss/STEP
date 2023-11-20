namespace StepLang.Expressions.Results;

/// <summary>
/// Represents a boolean (<c>true</c> or <c>false</c>).
/// </summary>
public class BoolResult : ValueExpressionResult<bool>
{
    /// <summary>
    /// A <see cref="BoolResult"/> with a value of <c>true</c>.
    /// </summary>
    public static readonly BoolResult True = new(true);

    /// <summary>
    /// A <see cref="BoolResult"/> with a value of <c>false</c>.
    /// </summary>
    public static readonly BoolResult False = new(false);

    /// <inheritdoc />
    public BoolResult(bool value) : base(ResultType.Bool, value)
    {
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is ExpressionResult result && EqualsInternal(result);

    /// <inheritdoc />
    public override int GetHashCode() => base.GetHashCode();

    /// <inheritdoc />
    protected override bool EqualsInternal(ExpressionResult other) => other is BoolResult boolResult && Value == boolResult.Value;

    /// <inheritdoc />
    public override BoolResult DeepClone() => Value ? True : False;

    /// <summary>
    /// Converts a <see cref="bool"/> to a <see cref="BoolResult"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The converted value.</returns>
    public static implicit operator BoolResult(bool value) => value ? True : False;

    /// <summary>
    /// Converts a <see cref="string"/> to a <see cref="BoolResult"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The converted value.</returns>
    public static implicit operator BoolResult(string value) => bool.Parse(value);

    /// <summary>
    /// Converts a <see cref="BoolResult"/> to a <see cref="bool"/>.
    /// </summary>
    /// <param name="result">The result to convert.</param>
    /// <returns>The converted value.</returns>
    public static implicit operator bool(BoolResult result) => result.Value;

    /// <summary>
    /// Inverts the value of a <see cref="BoolResult"/>.
    /// </summary>
    /// <param name="result">The result to convert.</param>
    /// <returns>The converted value.</returns>
    public static BoolResult operator !(BoolResult result) => !result.Value;

    /// <summary>
    /// Checks if two <see cref="BoolResult"/>s are equal.
    /// </summary>
    /// <param name="left">The left-hand side of the operator.</param>
    /// <param name="right">The right-hand side of the operator.</param>
    /// <returns><c>true</c> if the two <see cref="BoolResult"/>s are equal, <c>false</c> otherwise.</returns>
    public static BoolResult operator ==(BoolResult left, BoolResult right) => new(left.Value == right.Value);

    /// <summary>
    /// Checks if two <see cref="BoolResult"/>s are not equal.
    /// </summary>
    /// <param name="left">The left-hand side of the operator.</param>
    /// <param name="right">The right-hand side of the operator.</param>
    /// <returns><c>true</c> if the two <see cref="BoolResult"/>s are not equal, <c>false</c> otherwise.</returns>
    public static BoolResult operator !=(BoolResult left, BoolResult right) => new(left.Value != right.Value);
}