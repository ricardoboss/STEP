using System.Globalization;

namespace StepLang.Expressions.Results;

/// <summary>
/// Represents a number result and stores the value as a <see cref="double"/>.
/// </summary>
public class NumberResult : ComparableValueExpressionResult<double>
{
    /// <summary>
    /// Because of the way floating point numbers are represented, we need to use a very small value to compare them.
    /// Check the official docs for more info: https://learn.microsoft.com/en-us/dotnet/api/System.Double.Equals?view=net-7.0#precision-in-comparisons
    /// </summary>
    private const double Epsilon = 1e-32;

    /// <summary>
    /// A <see cref="NumberResult"/> with a value of 0.
    /// </summary>
    public static NumberResult Zero => new(0);

    /// <summary>
    /// Creates a new <see cref="NumberResult"/> with the given value.
    /// </summary>
    /// <param name="value">The value of the result.</param>
    public NumberResult(double value) : base(ResultType.Number, value)
    {
    }

    /// <inheritdoc />
    protected override int CompareToInternal(ComparableValueExpressionResult<double> other) => Value.CompareTo(other.Value);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is ExpressionResult result && EqualsInternal(result);

    /// <inheritdoc />
    protected override bool EqualsInternal(ExpressionResult other) => other is NumberResult numberResult && this == numberResult;

    /// <inheritdoc />
    public override int GetHashCode() => base.GetHashCode();

    /// <inheritdoc />
    public override NumberResult DeepClone() => new(Value);

    /// <inheritdoc />
    public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);

    /// <summary>
    /// Returns the result of adding two <see cref="NumberResult"/>s.
    /// </summary>
    /// <param name="left">The first <see cref="NumberResult"/>.</param>
    /// <param name="right">The second <see cref="NumberResult"/>.</param>
    /// <returns>The result of adding the two <see cref="NumberResult"/>s.</returns>
    public static NumberResult operator +(NumberResult left, NumberResult right) => new(left.Value + right.Value);

    /// <summary>
    /// Returns the result of subtracting two <see cref="NumberResult"/>s.
    /// </summary>
    /// <param name="left">The first <see cref="NumberResult"/>.</param>
    /// <param name="right">The second <see cref="NumberResult"/>.</param>
    /// <returns>The result of subtracting the two <see cref="NumberResult"/>s.</returns>
    public static NumberResult operator -(NumberResult left, NumberResult right) => new(left.Value - right.Value);

    /// <summary>
    /// Returns the result of multiplying two <see cref="NumberResult"/>s.
    /// </summary>
    /// <param name="left">The first <see cref="NumberResult"/>.</param>
    /// <param name="right">The second <see cref="NumberResult"/>.</param>
    /// <returns>The result of multiplying the two <see cref="NumberResult"/>s.</returns>
    public static NumberResult operator *(NumberResult left, NumberResult right) => new(left.Value * right.Value);

    /// <summary>
    /// Returns the result of dividing two <see cref="NumberResult"/>s.
    /// </summary>
    /// <param name="left">The first <see cref="NumberResult"/>.</param>
    /// <param name="right">The second <see cref="NumberResult"/>.</param>
    /// <returns>The result of dividing the two <see cref="NumberResult"/>s.</returns>
    public static NumberResult operator /(NumberResult left, NumberResult right) => new(left.Value / right.Value);

    /// <summary>
    /// Returns the remainder of dividing two <see cref="NumberResult"/>s.
    /// </summary>
    /// <param name="left">The first <see cref="NumberResult"/>.</param>
    /// <param name="right">The second <see cref="NumberResult"/>.</param>
    /// <returns>The remainder of dividing the two <see cref="NumberResult"/>s.</returns>
    public static NumberResult operator %(NumberResult left, NumberResult right) => new(left.Value % right.Value);

    /// <summary>
    /// Returns the result of negating a <see cref="NumberResult"/>.
    /// </summary>
    /// <param name="number">The <see cref="NumberResult"/> to negate.</param>
    /// <returns>The result of negating the <see cref="NumberResult"/>.</returns>
    public static NumberResult operator -(NumberResult number) => new(-number.Value);

    /// <summary>
    /// Compares the two <see cref="NumberResult"/>s and returns whether the left one is greater than the right one.
    /// </summary>
    /// <param name="left">The first <see cref="NumberResult"/>.</param>
    /// <param name="right">The second <see cref="NumberResult"/>.</param>
    /// <returns>Whether the left <see cref="NumberResult"/> is greater than the right one.</returns>
    public static BoolResult operator >(NumberResult left, NumberResult right) => left.Value > right.Value;

    /// <summary>
    /// Compares the two <see cref="NumberResult"/>s and returns whether the left one is less than the right one.
    /// </summary>
    /// <param name="left">The first <see cref="NumberResult"/>.</param>
    /// <param name="right">The second <see cref="NumberResult"/>.</param>
    /// <returns>Whether the left <see cref="NumberResult"/> is less than the right one.</returns>
    public static BoolResult operator <(NumberResult left, NumberResult right) => left.Value < right.Value;

    /// <summary>
    /// Compares the two <see cref="NumberResult"/>s and returns whether the left one is greater than or equal to the right one.
    /// </summary>
    /// <param name="left">The first <see cref="NumberResult"/>.</param>
    /// <param name="right">The second <see cref="NumberResult"/>.</param>
    /// <returns>Whether the left <see cref="NumberResult"/> is greater than or equal to the right one.</returns>
    public static BoolResult operator >=(NumberResult left, NumberResult right) => left.Value >= right.Value;

    /// <summary>
    /// Compares the two <see cref="NumberResult"/>s and returns whether the left one is less than or equal to the right one.
    /// </summary>
    /// <param name="left">The first <see cref="NumberResult"/>.</param>
    /// <param name="right">The second <see cref="NumberResult"/>.</param>
    /// <returns>Whether the left <see cref="NumberResult"/> is less than or equal to the right one.</returns>
    public static BoolResult operator <=(NumberResult left, NumberResult right) => left.Value <= right.Value;

    /// <summary>
    /// Compares the two <see cref="NumberResult"/>s and returns whether they are equal considering floating point arithmetics.
    /// </summary>
    /// <param name="left">The first <see cref="NumberResult"/>.</param>
    /// <param name="right">The second <see cref="NumberResult"/>.</param>
    /// <returns>Whether the two <see cref="NumberResult"/>s are equal.</returns>
    /// <seealso cref="Epsilon"/>
    public static BoolResult operator ==(NumberResult left, NumberResult right) => Math.Abs(left.Value - right.Value) < Epsilon;

    /// <summary>
    /// Compares the two <see cref="NumberResult"/>s and returns whether they are not equal considering floating point arithmetics.
    /// </summary>
    /// <param name="left">The first <see cref="NumberResult"/>.</param>
    /// <param name="right">The second <see cref="NumberResult"/>.</param>
    /// <returns>Whether the two <see cref="NumberResult"/>s are not equal.</returns>
    public static BoolResult operator !=(NumberResult left, NumberResult right) => Math.Abs(left.Value - right.Value) >= Epsilon;

    /// <summary>
    /// Converts a <see cref="double"/> to a <see cref="NumberResult"/>.
    /// </summary>
    /// <param name="value">The <see cref="double"/> to convert.</param>
    /// <returns>The converted <see cref="NumberResult"/>.</returns>
    public static implicit operator NumberResult(double value) => new(value);

    /// <summary>
    /// Converts an <see cref="int"/> to a <see cref="NumberResult"/>.
    /// </summary>
    /// <param name="value">The <see cref="int"/> to convert.</param>
    /// <returns>The converted <see cref="NumberResult"/>.</returns>
    public static implicit operator NumberResult(int value) => new(value);

    /// <summary>
    /// Converts a <see cref="string"/> to a <see cref="NumberResult"/> by first parsing the string as a <see cref="double"/>.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to convert.</param>
    /// <returns>The converted <see cref="NumberResult"/>.</returns>
    public static implicit operator NumberResult(string value) => double.Parse(value, NumberStyles.Any, CultureInfo.InvariantCulture);

    /// <summary>
    /// Converts a <see cref="NumberResult"/> to a <see cref="int"/> by rounding the value using <see cref="Math.Round(double)"/>.
    /// </summary>
    /// <param name="result">The <see cref="NumberResult"/> to convert.</param>
    /// <returns>The converted <see cref="int"/>.</returns>
    public static implicit operator int(NumberResult result) => (int)Math.Round(result.Value);

    /// <summary>
    /// Converts a <see cref="NumberResult"/> to a <see cref="uint"/> by rounding the value using <see cref="Math.Round(double)"/>.
    /// </summary>
    /// <param name="result">The <see cref="NumberResult"/> to convert.</param>
    /// <returns>The converted <see cref="uint"/>.</returns>
    public static implicit operator uint(NumberResult result) => (uint)Math.Round(result.Value);

    /// <summary>
    /// Converts a <see cref="NumberResult"/> to a <see cref="double"/>.
    /// </summary>
    /// <param name="result">The <see cref="NumberResult"/> to convert.</param>
    /// <returns>The converted <see cref="double"/>.</returns>
    public static implicit operator double(NumberResult result) => result.Value;

    /// <summary>
    /// Converts a <see cref="NumberResult"/> to a <see cref="string"/> by calling <see cref="double.ToString()"/>.
    /// </summary>
    /// <param name="result">The <see cref="NumberResult"/> to convert.</param>
    /// <returns>The converted <see cref="string"/>.</returns>
    public static implicit operator string(NumberResult result) => result.ToString();

    /// <summary>
    /// Creates a new <see cref="NumberResult"/> with the given value.
    /// </summary>
    /// <param name="value">The value to use.</param>
    /// <returns>The created <see cref="NumberResult"/>.</returns>
    public static NumberResult FromInt32(int value) => value;

    /// <summary>
    /// Creates a new <see cref="NumberResult"/> with the given value.
    /// </summary>
    /// <param name="value">The value to use.</param>
    /// <returns>The created <see cref="NumberResult"/>.</returns>
    public static NumberResult FromString(string value) => value;

    /// <summary>
    /// Converts the <see cref="NumberResult"/> to a <see cref="uint"/>.
    /// </summary>
    /// <returns>The converted <see cref="uint"/>.</returns>
    public uint ToUInt32() => this;

    /// <summary>
    /// Compares two <see cref="NumberResult"/>s.
    /// </summary>
    /// <param name="other">The other <see cref="NumberResult"/>.</param>
    /// <returns>The result of the comparison.</returns>
    /// <seealso cref="ComparableValueExpressionResult{T}.CompareTo(StepLang.Expressions.Results.ComparableValueExpressionResult{T}?)"/>
    public int CompareTo(NumberResult other) => base.CompareTo(other);
}