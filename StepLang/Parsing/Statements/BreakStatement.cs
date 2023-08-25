using StepLang.Interpreting;
using StepLang.Parsing.Expressions;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Statements;

public class BreakStatement : Statement
{
    private readonly Token breakToken;
    private readonly Expression expression;

    public BreakStatement(Token breakToken, Expression expression) : base(StatementType.BreakStatement)
    {
        this.breakToken = breakToken;
        this.expression = expression;

        Location = breakToken.Location;
    }

    public override async Task ExecuteAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        var breakDepthResult = await expression.EvaluateAsync(interpreter, cancellationToken);
        if (breakDepthResult is not NumberResult numberResult || numberResult.Value < 0 || !numberResult.IsInteger)
            throw new InvalidDepthResultException(breakToken, breakDepthResult);

        interpreter.BreakDepth += (int)numberResult.Value;
    }
}