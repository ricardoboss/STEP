using System.Diagnostics.CodeAnalysis;
using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Expressions;

public class LiteralExpression : Expression
{
    public static LiteralExpression Str(string value) => new(new StringResult(value));
    public static LiteralExpression Number(double value) => new(new NumberResult(value));
    public static LiteralExpression Bool(bool value) => new(new BoolResult(value));
    public static LiteralExpression List(IList<ExpressionResult> value) => new(new ListResult(value));
    public static LiteralExpression Map(IDictionary<string, ExpressionResult> value) => new(new MapResult(value));
    public static LiteralExpression True => Bool(true);
    public static LiteralExpression False => Bool(false);
    public static LiteralExpression Null => new(NullResult.Instance);
    public static LiteralExpression Void => new(VoidResult.Instance);
    public static LiteralExpression From(ExpressionResult result) => new(result);

    private LiteralExpression(ExpressionResult result) => Result = result;

    public override Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, CancellationToken cancellationToken = default) => Task.FromResult(Result);

    public ExpressionResult Result { get; }

    [ExcludeFromCodeCoverage]
    protected override string DebugDisplay() => Result.ToString();
}