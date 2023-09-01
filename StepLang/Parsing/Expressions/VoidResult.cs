namespace StepLang.Parsing.Expressions;

public class VoidResult : ExpressionResult
{
    public static readonly VoidResult Instance = new();

    /// <inheritdoc />
    private VoidResult() : base(ResultType.Void)
    {
    }

    public override string ToString() => "void";
}