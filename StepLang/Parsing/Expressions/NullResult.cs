namespace StepLang.Parsing.Expressions;

public class NullResult : ExpressionResult
{
    public static readonly NullResult Instance = new();

    /// <inheritdoc />
    private NullResult() : base(ResultType.Null)
    {
    }

    public override string ToString() => "null";
}