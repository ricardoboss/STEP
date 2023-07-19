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

    public Task InitializeLoopAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        interpreter.PushScope();

        return Task.CompletedTask;
    }

    public async Task<bool> ShouldLoopAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        var result = await condition.EvaluateAsync(interpreter, default);

        return result is { ValueType: "bool", Value: true };
    }

    public async Task ExecuteLoopAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        await interpreter.InterpretAsync(statements.ToAsyncEnumerable(), cancellationToken);
    }

    public Task FinalizeLoopAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        interpreter.PopScope();

        return Task.CompletedTask;
    }

    protected override string DebugRenderContent()
    {
        return $"{condition} {{ [{statements.Count}] }}";
    }
}