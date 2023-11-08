using System.Globalization;

namespace StepLang.Expressions.Results;

public class NumberResult : ComparableValueExpressionResult<double>
{
    /// <summary>
    /// Because of the way floating point numbers are represented, we need to use a very small value to compare them.
    /// Check the official docs for more info: https://learn.microsoft.com/en-us/dotnet/api/System.Double.Equals?view=net-7.0#precision-in-comparisons
    /// </summary>
    private const double Epsilon = 1e-32;

    public static NumberResult Zero => new(0);

    /// <inheritdoc />
    public NumberResult(double value) : base(ResultType.Number, value)
    {
    }

    protected override int CompareToInternal(ComparableValueExpressionResult<double> other) => Value.CompareTo(other.Value);

    public override bool Equals(object? obj) => obj is ExpressionResult result && EqualsInternal(result);

    protected override bool EqualsInternal(ExpressionResult other) => other is NumberResult numberResult && this == numberResult;

    public override int GetHashCode() => base.GetHashCode();

    public override NumberResult DeepClone() => new(Value);

    /// <inheritdoc />
    public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);

    public static NumberResult operator +(NumberResult left, NumberResult right) => new(left.Value + right.Value);

    public static NumberResult operator -(NumberResult left, NumberResult right) => new(left.Value - right.Value);

    public static NumberResult operator *(NumberResult left, NumberResult right) => new(left.Value * right.Value);

    public static NumberResult operator /(NumberResult left, NumberResult right) => new(left.Value / right.Value);

    public static NumberResult operator %(NumberResult left, NumberResult right) => new(left.Value % right.Value);

    public static NumberResult operator -(NumberResult number) => new(-number.Value);

    public static BoolResult operator >(NumberResult left, NumberResult right) => left.Value > right.Value;

    public static BoolResult operator <(NumberResult left, NumberResult right) => left.Value < right.Value;

    public static BoolResult operator >=(NumberResult left, NumberResult right) => left.Value >= right.Value;

    public static BoolResult operator <=(NumberResult left, NumberResult right) => left.Value <= right.Value;

    public static BoolResult operator ==(NumberResult left, NumberResult right) => Math.Abs(left.Value - right.Value) < Epsilon;

    public static BoolResult operator !=(NumberResult left, NumberResult right) => Math.Abs(left.Value - right.Value) >= Epsilon;

    public static implicit operator NumberResult(double value) => new(value);

    public static implicit operator NumberResult(int value) => new(value);

    public static implicit operator NumberResult(string value) => double.Parse(value, NumberStyles.Any, CultureInfo.InvariantCulture);

    public static implicit operator int(NumberResult result) => (int)Math.Round(result.Value);

    public static implicit operator double(NumberResult result) => result.Value;

    public static implicit operator string(NumberResult result) => result.ToString();

    public static NumberResult FromDouble(double value) => value;

    public static NumberResult FromInt32(int value) => value;

    public static NumberResult FromString(string value) => value;

    public int ToInt32() => this;

    public double ToDouble() => this;

    public static NumberResult Add(NumberResult left, NumberResult right) => left + right;

    public static NumberResult Subtract(NumberResult left, NumberResult right) => left - right;

    public static NumberResult Multiply(NumberResult left, NumberResult right) => left * right;

    public static NumberResult Divide(NumberResult left, NumberResult right) => left / right;

    public static NumberResult Remainder(NumberResult left, NumberResult right) => left % right;

    public static NumberResult Negate(NumberResult number) => -number;

    public int CompareTo(NumberResult other) => base.CompareTo(other);
}