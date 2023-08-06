using STEP.Interpreting;
using STEP.Parsing.Expressions;

namespace STEP.Parsing.Statements;

internal class ContinueStatement : Statement
{
    private readonly Expression expression;

    public ContinueStatement(Expression expression) : base(StatementType.ContinueStatement)
    {
        this.expression = expression;
    }

    public override async Task ExecuteAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        var continueDepthResult = await expression.EvaluateAsync(interpreter, cancellationToken);
        if (continueDepthResult is not { ValueType: "number" } or { Value: <= 0 })
            throw new InvalidDepthResult("continue", continueDepthResult);

        interpreter.ContinueDepth += continueDepthResult.Value;
    }
}