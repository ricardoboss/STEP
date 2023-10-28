namespace StepLang.Expressions.Results;

/// <summary>
/// Represents a result that has no value.
/// </summary>
public class VoidResult : ExpressionResult
{
    /// <summary>
    /// The singleton instance of <see cref="VoidResult"/>.
    /// </summary>
    public static readonly VoidResult Instance = new();

    private VoidResult() : base(ResultType.Void)
    {
    }

    /// <inheritdoc />
    protected override bool EqualsInternal(ExpressionResult other)
    {
        return other is VoidResult;
    }

    /// <inheritdoc />
    public override ExpressionResult DeepClone()
    {
        return Instance;
    }

    /// <inheritdoc />
    public override string ToString() => "{void}";
}