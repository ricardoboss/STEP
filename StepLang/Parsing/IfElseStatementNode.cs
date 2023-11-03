namespace StepLang.Parsing;

public sealed record IfElseStatementNode(ExpressionNode Condition, IReadOnlyCollection<StatementNode> Body, IReadOnlyCollection<StatementNode> ElseBody) : StatementNode
{
    public override void Accept(IStatementVisitor visitor)
    {
        visitor.Execute(this);
    }
}