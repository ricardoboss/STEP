namespace StepLang.Parsing;

public sealed record WhileStatementNode(ExpressionNode Condition, IReadOnlyCollection<StatementNode> Body) : StatementNode
{
    public override void Accept(IStatementVisitor visitor)
    {
        visitor.Execute(this);
    }
}