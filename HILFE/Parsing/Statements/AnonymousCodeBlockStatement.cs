using HILFE.Interpreting;

namespace HILFE.Parsing.Statements;

public class AnonymousCodeBlockStatement : Statement, IExecutableStatement
{
    private readonly IReadOnlyList<Statement> statements;

    public AnonymousCodeBlockStatement(IReadOnlyList<Statement> statements) : base(StatementType.AnonymousCodeBlock)
    {
        this.statements = statements;
    }

    /// <inheritdoc />
    public async Task ExecuteAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        interpreter.PushScope();

        await interpreter.InterpretAsync(statements.ToAsyncEnumerable(), cancellationToken);

        interpreter.PopScope();
    }
}