using HILFE.Interpreting;

namespace HILFE.Parsing.Expressions;

public class ConstantExpression : Expression
{
    public static ConstantExpression Null { get; } = new(ExpressionResult.Null);

    public static ConstantExpression Void { get; } = new(ExpressionResult.Void);

    public static ConstantExpression True { get; } = new(ExpressionResult.True);
    
    public static ConstantExpression False { get; } = new(ExpressionResult.False);

    public static ConstantExpression Number(double value) => new(ExpressionResult.Number(value));

    public static ConstantExpression String(string value) => new(ExpressionResult.String(value));

    public static ConstantExpression Bool(bool value) => new(ExpressionResult.Bool(value));

    private readonly ExpressionResult result;

    public ConstantExpression(ExpressionResult result)
    {
        this.result = result;
    }

    public override Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(result);
    }

    protected override string DebugDisplay() => result.ToString();
}