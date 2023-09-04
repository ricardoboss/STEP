namespace StepLang.Parsing.Expressions;

public class StringResult : ComparableValueExpressionResult<string>
{
    public static StringResult Empty => new(string.Empty);

    /// <inheritdoc />
    public StringResult(string value) : base(ResultType.Str, value)
    {
    }

    protected override int CompareToInternal(ComparableValueExpressionResult<string> other)
    {
        return string.Compare(Value, other.Value, StringComparison.Ordinal);
    }

    protected override bool EqualsInternal(ExpressionResult other)
    {
        return other is StringResult stringResult && string.Equals(Value, stringResult.Value, StringComparison.Ordinal);
    }
}