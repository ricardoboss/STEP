namespace StepLang.Parsing.Expressions;

public class BoolResult : ValueExpressionResult<bool>
{
    public static readonly BoolResult True = new(true);

    public static readonly BoolResult False = new(false);

    /// <inheritdoc />
    public BoolResult(bool value) : base(ResultType.Bool, value)
    {
    }

    public override string ToString() => Value.ToString();
}