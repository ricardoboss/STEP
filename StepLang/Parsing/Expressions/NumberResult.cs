namespace StepLang.Parsing.Expressions;

public class NumberResult : ValueExpressionResult<double>
{
    public static readonly NumberResult Zero = new(0);

    /// <inheritdoc />
    public NumberResult(double value) : base(ResultType.Number, value)
    {
    }

    public bool IsInteger => Math.Abs(Value % 1) < double.Epsilon * 100;

    public int RoundedIntValue => (int)Math.Round(Value);
}