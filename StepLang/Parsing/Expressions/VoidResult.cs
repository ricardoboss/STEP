namespace StepLang.Parsing.Expressions;

public class VoidResult : ExpressionResult
{
    public static readonly VoidResult Instance = new();

    /// <inheritdoc />
    private VoidResult() : base(ResultType.Void)
    {
    }

    protected override bool EqualsInternal(ExpressionResult other)
    {
        return other is VoidResult;
    }
}