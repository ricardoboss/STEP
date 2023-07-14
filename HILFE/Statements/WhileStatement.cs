namespace HILFE.Statements;

public class WhileStatement : BaseStatement
{
    /// <inheritdoc />
    public WhileStatement(IReadOnlyList<Token> tokens) : base(StatementType.IfStatement, tokens)
    {
    }

    /// <inheritdoc />
    public override Task ExecuteAsync(Interpreter interpreter)
    {
        return Task.CompletedTask;
    }
}