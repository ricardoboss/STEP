namespace HILFE.Statements;

public class EmptyLineStatement : BaseStatement
{
    /// <inheritdoc />
    public EmptyLineStatement(IReadOnlyList<Token> tokens) : base(StatementType.EmptyLine, tokens)
    {
    }

    /// <inheritdoc />
    public override Task ExecuteAsync(Interpreter interpreter)
    {
        return Task.CompletedTask;
    }
}