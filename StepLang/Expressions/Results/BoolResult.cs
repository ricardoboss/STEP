namespace StepLang.Expressions.Results;

public class BoolResult : ValueExpressionResult<bool>
{
    public static readonly BoolResult True = new(true);

    public static readonly BoolResult False = new(false);

    /// <inheritdoc />
    public BoolResult(bool value) : base(ResultType.Bool, value)
    {
    }

    protected override bool EqualsInternal(ExpressionResult other)
    {
        return other is BoolResult boolResult && Value == boolResult.Value;
    }

    public override BoolResult DeepClone()
    {
        return new(Value);
    }

    public static implicit operator BoolResult(bool value) => new(value);

    public static implicit operator BoolResult(string value) => bool.Parse(value);

    public static implicit operator bool(BoolResult result) => result.Value;

    public static BoolResult operator !(BoolResult result) => new(!result.Value);

    public static BoolResult operator ==(BoolResult left, BoolResult right) => new(left.Value == right.Value);

    public static BoolResult operator !=(BoolResult left, BoolResult right) => new(left.Value != right.Value);
}