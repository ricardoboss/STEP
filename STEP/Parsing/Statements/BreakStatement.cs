using STEP.Interpreting;
using STEP.Parsing.Expressions;

namespace STEP.Parsing.Statements;

public class BreakStatement: Statement
{
    private readonly Expression expression;

    public BreakStatement(Expression expression) : base(StatementType.BreakStatement)
    {
        this.expression = expression;
    }

    public override async Task ExecuteAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        var breakDepthResult = await expression.EvaluateAsync(interpreter, cancellationToken);
        if (breakDepthResult is not { ValueType: "number" } or { Value: <= 0 })
            throw new InvalidDepthResult("break", breakDepthResult);

        interpreter.BreakDepth += breakDepthResult.Value;
    }
}