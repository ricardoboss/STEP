using HILFE.Interpreting;

namespace HILFE.Parsing.Expressions;

public class ConstantExpression : Expression
{
    private readonly ExpressionResult result;

    public ConstantExpression(string type, dynamic? value)
    {
        result = new(type, value);
    }

    public override Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(result);
    }

    protected override string DebugDisplay() => result.ToString();
}