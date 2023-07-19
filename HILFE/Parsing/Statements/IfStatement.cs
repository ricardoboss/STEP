using HILFE.Interpreting;
using HILFE.Tokenizing;

namespace HILFE.Parsing.Statements;

public class IfStatement : Statement, IBranchingStatement
{
    private readonly Expression condition;
    private readonly IReadOnlyList<Statement> trueBranch;
    private readonly IReadOnlyList<Statement>? falseBranch;

    /// <inheritdoc />
    public IfStatement(Expression condition, IReadOnlyList<Statement> trueBranch, IReadOnlyList<Statement>? falseBranch) : base(StatementType.IfStatement)
    {
        this.condition = condition;
        this.trueBranch = trueBranch;
        this.falseBranch = falseBranch;
    }

    public async Task<bool> ShouldBranch(Interpreter interpreter)
    {
        var result = await condition.EvaluateAsync(interpreter, default);

        return result.Value == true;
    }

    public async Task ExecuteTrueBranch(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        await interpreter.InterpretAsync(trueBranch.ToAsyncEnumerable(), cancellationToken);
    }

    public async Task ExecuteFalseBranch(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        if (falseBranch is null)
            return;

        await interpreter.InterpretAsync(falseBranch.ToAsyncEnumerable(), cancellationToken);
    }
}