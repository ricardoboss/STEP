using HILFE.Interpreting;
using HILFE.Tokenizing;

namespace HILFE.Parsing.Statements;

public class IfElseStatement : Statement
{
    private readonly Expression condition;
    private readonly IReadOnlyList<Statement> trueBranch;
    private readonly IReadOnlyList<Statement>? falseBranch;

    /// <inheritdoc />
    public IfElseStatement(Expression condition, IReadOnlyList<Statement> trueBranch, IReadOnlyList<Statement>? falseBranch) : base(StatementType.IfElseStatement)
    {
        this.condition = condition;
        this.trueBranch = trueBranch;
        this.falseBranch = falseBranch;
    }

    public Task<bool> ShouldBranch(Interpreter interpreter)
    {
        return condition.Evaluate(interpreter).Value == true;
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