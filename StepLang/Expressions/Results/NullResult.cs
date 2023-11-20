namespace StepLang.Expressions.Results;

/// <summary>
/// Represents a <see langword="null"/> value.
/// </summary>
public class NullResult : ExpressionResult
{
    /// <summary>
    /// The singleton instance of the null result.
    /// </summary>
    public static readonly NullResult Instance = new();

    private NullResult() : base(ResultType.Null)
    {
    }

    /// <inheritdoc />
    protected override bool EqualsInternal(ExpressionResult other) => other is NullResult;

    /// <inheritdoc />
    public override ExpressionResult DeepClone() => Instance;

    /// <inheritdoc />
    public override string ToString() => "{null}";
}