using System.Globalization;

namespace StepLang.Expressions.Results;

public class NumberResult : ComparableValueExpressionResult<double>
{
    private const double Epsilon = 1e-32;

    public static NumberResult Zero => new(0);

    /// <inheritdoc />
    public NumberResult(double value) : base(ResultType.Number, value)
    {
    }

    protected override int CompareToInternal(ComparableValueExpressionResult<double> other) => Value.CompareTo(other.Value);

    protected override bool EqualsInternal(ExpressionResult other) => other is NumberResult numberResult && this == numberResult;

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
}