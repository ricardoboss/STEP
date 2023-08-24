using StepLang.Interpreting;

namespace StepLang.Parsing.Expressions;

public class BinaryExpression : Expression
{
    private readonly string debugName;
    private readonly Expression left;
    private readonly Expression right;
    private readonly Func<ExpressionResult, ExpressionResult, ExpressionResult> combine;

    public BinaryExpression(string debugName, Expression left, Expression right, Func<ExpressionResult, ExpressionResult, ExpressionResult> combine)
    {
        this.debugName = debugName;
        this.left = left;
        this.right = right;
        this.combine = combine;
    }

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        var leftValue = await left.EvaluateAsync(interpreter, cancellationToken);
        var rightValue = await right.EvaluateAsync(interpreter, cancellationToken);

        return combine.Invoke(leftValue, rightValue);
    }

    protected override string DebugDisplay() => $"({left}) {debugName} ({right})";
}