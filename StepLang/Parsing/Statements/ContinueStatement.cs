using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Parsing.Statements;

internal sealed class ContinueStatement : Statement
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