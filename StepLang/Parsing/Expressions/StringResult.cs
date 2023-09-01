namespace StepLang.Parsing.Expressions;

public class StringResult : ValueExpressionResult<string>
{
    public static readonly StringResult Empty = new(string.Empty);

    /// <inheritdoc />
    public StringResult(string value) : base(ResultType.Str, value)
    {
    }

    public override string ToString() => $"\"{Value}\"";
}