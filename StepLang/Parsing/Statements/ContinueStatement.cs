using StepLang.Interpreting;
using StepLang.Parsing.Expressions;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Statements;

internal sealed class ContinueStatement : Statement
{
    private readonly Token continueToken;
    private readonly Expression expression;

    public ContinueStatement(Token continueToken, Expression expression) : base(StatementType.ContinueStatement)
    {
        this.continueToken = continueToken;
        this.expression = expression;

        Location = continueToken.Location;
    }

    public override async Task ExecuteAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        var continueDepthResult = await expression.EvaluateAsync(interpreter, cancellationToken);
        if (continueDepthResult is not { ValueType: "number" } or { Value: <= 0 })
            throw new InvalidDepthResult(continueToken, continueDepthResult);

        interpreter.ContinueDepth += continueDepthResult.Value;
    }
}