namespace StepLang.Expressions.Results;

public class BoolResult : ValueExpressionResult<bool>
{
	public static readonly BoolResult True = new(true);

	public static readonly BoolResult False = new(false);

	/// <inheritdoc />
	public BoolResult(bool value) : base(ResultType.Bool, value)
	{
	}

	public override bool Equals(object? obj)
	{
		return obj is ExpressionResult result && EqualsInternal(result);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	protected override bool EqualsInternal(ExpressionResult other)
	{
		return other is BoolResult boolResult && Value == boolResult.Value;
	}

	public override BoolResult DeepClone()
	{
		return Value ? True : False;
	}

	public static implicit operator BoolResult(bool value)
	{
		return value ? True : False;
	}

	public static implicit operator BoolResult(string value)
	{
		return bool.Parse(value);
	}

	public static implicit operator bool(BoolResult result)
	{
		return result.Value;
	}

	public static BoolResult operator !(BoolResult result)
	{
		return !result.Value;
	}

	public static BoolResult operator ==(BoolResult left, BoolResult right)
	{
		return new BoolResult(left.Value == right.Value);
	}

	public static BoolResult operator !=(BoolResult left, BoolResult right)
	{
		return new BoolResult(left.Value != right.Value);
	}

	public static BoolResult FromBoolean(bool value)
	{
		return value;
	}

	public static BoolResult FromString(string value)
	{
		return value;
	}

	public bool ToBoolean()
	{
		return Value;
	}

	public BoolResult LogicalNot()
	{
		return !Value;
	}

	public static BoolResult Equals(BoolResult left, BoolResult right)
	{
		return left == right;
	}

	public static BoolResult NotEquals(BoolResult left, BoolResult right)
	{
		return left != right;
	}

	public override string ToString() => Value.ToString();
}
