using HILFE.Interpreting;

namespace HILFE.Parsing.Statements;

public class IfElseStatement : Statement, IExecutableStatement
{
    private readonly Expression firstCondition;
    private readonly Expression? secondCondition;
    private readonly IReadOnlyList<Statement> firstBranch;
    private readonly IReadOnlyList<Statement> secondBranch;

    /// <inheritdoc />
    public IfElseStatement(Expression firstCondition, IReadOnlyList<Statement> firstBranch, Expression? secondCondition, IReadOnlyList<Statement> secondBranch) : base(StatementType.IfElseStatement)
    {
        this.firstCondition = firstCondition;
        this.firstBranch = firstBranch;
        this.secondCondition = secondCondition;
        this.secondBranch = secondBranch;
    }

    public async Task ExecuteAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        if (await ShouldExecuteFirstBranch(interpreter, cancellationToken))
        {
            await ExecuteFirstBranch(interpreter, cancellationToken);

            return;
        }

        if (await ShouldExecuteSecondBranch(interpreter, cancellationToken))
            await ExecuteSecondBranch(interpreter, cancellationToken);
    }

    private async Task<bool> ShouldExecuteFirstBranch(Interpreter interpreter,
        CancellationToken cancellationToken = default)
    {
        var result = await firstCondition.EvaluateAsync(interpreter, cancellationToken);

        return result is { ValueType: "bool", Value: true };
    }

    private async Task<bool> ShouldExecuteSecondBranch(Interpreter interpreter,
        CancellationToken cancellationToken = default)
    {
        if (secondCondition is null)
            return true;

        var result = await secondCondition.EvaluateAsync(interpreter, cancellationToken);

        return result is { ValueType: "bool", Value: true };
    }

    private async Task ExecuteFirstBranch(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        await interpreter.InterpretAsync(firstBranch.ToAsyncEnumerable(), cancellationToken);
    }

    private async Task ExecuteSecondBranch(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        await interpreter.InterpretAsync(secondBranch.ToAsyncEnumerable(), cancellationToken);
    }
}