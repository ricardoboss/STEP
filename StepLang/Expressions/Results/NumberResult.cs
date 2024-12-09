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

	protected override int CompareToInternal(ComparableValueExpressionResult<double> other)
	{
		return Value.CompareTo(other.Value);
	}

	public override bool Equals(object? obj)
	{
		return obj is ExpressionResult result && EqualsInternal(result);
	}

	protected override bool EqualsInternal(ExpressionResult other)
	{
		return other is NumberResult numberResult && this == numberResult;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override NumberResult DeepClone()
	{
		return new NumberResult(Value);
	}

	/// <inheritdoc />
	public override string ToString()
	{
		return Value.ToString(CultureInfo.InvariantCulture);
	}

	public static NumberResult operator +(NumberResult left, NumberResult right)
	{
		return new NumberResult(left.Value + right.Value);
	}

	public static NumberResult operator -(NumberResult left, NumberResult right)
	{
		return new NumberResult(left.Value - right.Value);
	}

	public static NumberResult operator *(NumberResult left, NumberResult right)
	{
		return new NumberResult(left.Value * right.Value);
	}

	public static NumberResult operator /(NumberResult left, NumberResult right)
	{
		return new NumberResult(left.Value / right.Value);
	}

	public static NumberResult operator %(NumberResult left, NumberResult right)
	{
		return new NumberResult(left.Value % right.Value);
	}

	public static NumberResult operator -(NumberResult number)
	{
		return new NumberResult(-number.Value);
	}

	public static BoolResult operator >(NumberResult left, NumberResult right)
	{
		return left.Value > right.Value;
	}

	public static BoolResult operator <(NumberResult left, NumberResult right)
	{
		return left.Value < right.Value;
	}

	public static BoolResult operator >=(NumberResult left, NumberResult right)
	{
		return left.Value >= right.Value;
	}

	public static BoolResult operator <=(NumberResult left, NumberResult right)
	{
		return left.Value <= right.Value;
	}

	public static BoolResult operator ==(NumberResult left, NumberResult right)
	{
		return Math.Abs(left.Value - right.Value) < Epsilon;
	}

	public static BoolResult operator !=(NumberResult left, NumberResult right)
	{
		return Math.Abs(left.Value - right.Value) >= Epsilon;
	}

	public static implicit operator NumberResult(double value)
	{
		return new NumberResult(value);
	}

	public static implicit operator NumberResult(int value)
	{
		return new NumberResult(value);
	}

	public static implicit operator NumberResult(string value)
	{
		return double.Parse(value, NumberStyles.Any, CultureInfo.InvariantCulture);
	}

	public static implicit operator int(NumberResult result)
	{
		return (int)Math.Round(result.Value);
	}

	public static implicit operator uint(NumberResult result)
	{
		return (uint)Math.Round(result.Value);
	}

	public static implicit operator double(NumberResult result)
	{
		return result.Value;
	}

	public static implicit operator string(NumberResult result)
	{
		return result.ToString();
	}

	public static NumberResult FromDouble(double value)
	{
		return value;
	}

	public static NumberResult FromInt32(int value)
	{
		return value;
	}

	public static NumberResult FromString(string value)
	{
		return value;
	}

	public int ToInt32()
	{
		return this;
	}

	public uint ToUInt32()
	{
		return this;
	}

	public double ToDouble()
	{
		return this;
	}

	public static NumberResult Add(NumberResult left, NumberResult right)
	{
		return left + right;
	}

	public static NumberResult Subtract(NumberResult left, NumberResult right)
	{
		return left - right;
	}

	public static NumberResult Multiply(NumberResult left, NumberResult right)
	{
		return left * right;
	}

	public static NumberResult Divide(NumberResult left, NumberResult right)
	{
		return left / right;
	}

	public static NumberResult Remainder(NumberResult left, NumberResult right)
	{
		return left % right;
	}

	public static NumberResult Negate(NumberResult number)
	{
		return -number;
	}

	public int CompareTo(NumberResult other)
	{
		return base.CompareTo(other);
	}
}
