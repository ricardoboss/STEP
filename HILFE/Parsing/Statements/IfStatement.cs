using HILFE.Interpreting;

namespace HILFE.Parsing.Statements;

public class IfStatement : Statement, IExecutableStatement
{
    private readonly Expression condition;
    private readonly IReadOnlyList<Statement> statements;

    /// <inheritdoc />
    public IfStatement(Expression condition, IReadOnlyList<Statement> statements) : base(StatementType.IfStatement)
    {
        this.condition = condition;
        this.statements = statements;
    }

    private async Task<bool> ShouldEnterBranch(Interpreter interpreter, CancellationToken cancellationToken)
    {
        var result = await condition.EvaluateAsync(interpreter, cancellationToken);

        return result is { ValueType: "bool", Value: true };
    }

    private async Task ExecuteBranch(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        await interpreter.InterpretAsync(statements.ToAsyncEnumerable(), cancellationToken);
    }

    public async Task ExecuteAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        if (!await ShouldEnterBranch(interpreter, cancellationToken))
            return;

        await ExecuteBranch(interpreter, cancellationToken);
    }
}