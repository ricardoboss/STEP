using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;

namespace StepLang.Expressions;

public abstract class BinaryExpression : Expression
{
    private readonly Expression leftExpression;
    private readonly Expression rightExpression;
    private readonly BinaryExpressionOperator @operator;

    protected BinaryExpression(Expression leftExpression, Expression rightExpression, BinaryExpressionOperator @operator)
    {
        this.leftExpression = leftExpression;
        this.rightExpression = rightExpression;
        this.@operator = @operator;
    }

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        var leftValue = await leftExpression.EvaluateAsync(interpreter, cancellationToken);
        var rightValue = await rightExpression.EvaluateAsync(interpreter, cancellationToken);

        return Combine(leftValue, rightValue);
    }

    protected override string DebugDisplay() => $"({leftExpression}) {@operator.ToSymbol()} ({rightExpression})";

    protected abstract ExpressionResult Combine(ExpressionResult left, ExpressionResult right);
}