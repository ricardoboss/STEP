namespace StepLang.Parsing.Expressions;

public class NumberResult : ComparableValueExpressionResult<double>
{
    public static NumberResult Zero => new(0);

    /// <inheritdoc />
    public NumberResult(double value) : base(ResultType.Number, value)
    {
    }

    public bool IsInteger => Math.Abs(Value % 1) < double.Epsilon * 100;

    public int RoundedIntValue => (int)Math.Round(Value);

    protected override int CompareToInternal(ComparableValueExpressionResult<double> other)
    {
        return Value.CompareTo(other.Value);
    }

    protected override bool EqualsInternal(ExpressionResult other)
    {
        return other is NumberResult numberResult && Value.Equals(numberResult.Value);
    }
}