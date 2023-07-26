using HILFE.Interpreting;

namespace HILFE.Parsing.Expressions;

public class ConstantExpression : Expression
{
    public static ConstantExpression Null { get; } = new(ExpressionResult.Null);

    public static ConstantExpression Void { get; } = new(ExpressionResult.Void);

    public static ConstantExpression True { get; } = new("bool", true);
    
    public static ConstantExpression False { get; } = new("bool", false);

    public static ConstantExpression Number(double value) => new("number", value);

    public static ConstantExpression String(string value) => new("string", value);

    public static ConstantExpression Bool(bool value) => new("bool", value);

    private readonly ExpressionResult result;

    public ConstantExpression(string type, dynamic? value)
    {
        result = new(type, value);
    }

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