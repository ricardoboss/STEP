namespace HILFE.Statements;

public class ElseStatement : BaseStatement
{
    /// <inheritdoc />
    public ElseStatement(IReadOnlyList<Token> tokens) : base(StatementType.ElseStatement, tokens)
    {
    }

    /// <inheritdoc />
    public override Task ExecuteAsync(Interpreter interpreter)
    {
        return Task.CompletedTask;
    }
}