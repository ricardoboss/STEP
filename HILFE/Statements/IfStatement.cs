namespace HILFE.Statements;

public class IfStatement : BaseStatement
{
    /// <inheritdoc />
    public IfStatement(IReadOnlyList<Token> tokens) : base(StatementType.IfStatement, tokens)
    {
    }

    /// <inheritdoc />
    public override Task ExecuteAsync(Interpreter interpreter)
    {
        return Task.CompletedTask;
    }
}