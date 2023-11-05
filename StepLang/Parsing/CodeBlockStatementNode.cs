namespace StepLang.Parsing;

public sealed record CodeBlockStatementNode(IReadOnlyList<StatementNode> Body) : StatementNode
{
    public override void Accept(IStatementVisitor visitor)
    {
        visitor.Execute(this);
    }
}
