using System.Diagnostics.CodeAnalysis;
using StepLang.Interpreting;

namespace StepLang.Parsing.Expressions;

public class ConstantExpression : Expression
{
    public static ConstantExpression Str(string value) => new(new StringResult(value));
    public static ConstantExpression Number(double value) => new(new NumberResult(value));
    public static ConstantExpression Bool(bool value) => new(new BoolResult(value));
    public static ConstantExpression True => Bool(true);
    public static ConstantExpression False => Bool(false);
    public static ConstantExpression Null => new(NullResult.Instance);
    public static ConstantExpression Void => new(VoidResult.Instance);

    public ConstantExpression(ExpressionResult result) => Result = result;

    public override Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, CancellationToken cancellationToken = default) => Task.FromResult(Result);

    public ExpressionResult Result { get; }

    [ExcludeFromCodeCoverage]
    protected override string DebugDisplay() => Result.ToString();
}