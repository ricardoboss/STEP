namespace StepLang.Parsing;

public sealed record CodeBlockStatementNode(IReadOnlyCollection<StatementNode> Body) : StatementNode
{
    public override void Accept(IStatementVisitor visitor)
    {
        visitor.Execute(this);
    }
}
