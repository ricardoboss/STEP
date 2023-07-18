using HILFE.Interpreting;

namespace HILFE.Parsing.Statements;

public class WhileStatement : Statement, ILoopingStatement
{
    private readonly Expression condition;
    private readonly IReadOnlyList<Statement> statements;

    /// <inheritdoc />
    public WhileStatement(Expression condition, IReadOnlyList<Statement> statements) : base(StatementType.WhileStatement)
    {
        this.condition = condition;
        this.statements = statements;
    }

    public Task InitializeLoop(Interpreter interpreter)
    {
        return Task.CompletedTask;
    }

    public Task<bool> ShouldLoop(Interpreter interpreter)
    {
        return Task.FromResult<bool>(condition.Evaluate(interpreter).Value == true);
    }

    public async Task ExecuteLoop(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        await interpreter.InterpretAsync(statements.ToAsyncEnumerable(), cancellationToken);
    }
}