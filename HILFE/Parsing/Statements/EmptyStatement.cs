namespace HILFE.Parsing.Statements;

public class EmptyStatement : Statement
{
    /// <inheritdoc />
    public EmptyStatement() : base(StatementType.EmptyStatement)
    {
    }
}