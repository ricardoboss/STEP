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

    public override bool Equals(object? obj) => obj is ExpressionResult result && EqualsInternal(result);

    public override int GetHashCode() => base.GetHashCode();

    protected override bool EqualsInternal(ExpressionResult other) => other is BoolResult boolResult && Value == boolResult.Value;

    /// <inheritdoc />
    public override BoolResult DeepClone() => Value ? True : False;

    public static implicit operator BoolResult(bool value) => value ? True : False;

    public static implicit operator BoolResult(string value) => bool.Parse(value);

    public static implicit operator bool(BoolResult result) => result.Value;

    public static BoolResult operator !(BoolResult result) => !result.Value;

    public static BoolResult operator ==(BoolResult left, BoolResult right) => new(left.Value == right.Value);

    public static BoolResult operator !=(BoolResult left, BoolResult right) => new(left.Value != right.Value);

    public static BoolResult FromBoolean(bool value) => value;

    public static BoolResult FromString(string value) => value;

    public bool ToBoolean() => Value;

    public BoolResult LogicalNot() => !Value;

    public static BoolResult Equals(BoolResult left, BoolResult right) => left == right;

    public static BoolResult NotEquals(BoolResult left, BoolResult right) => left != right;
}