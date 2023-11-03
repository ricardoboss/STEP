namespace StepLang.Parsing;

public sealed record IfElseIfStatementNode(ExpressionNode Condition, IReadOnlyCollection<StatementNode> Body, ExpressionNode ElseCondition, IReadOnlyCollection<StatementNode> ElseBody) : StatementNode
{
    public override void Accept(IStatementVisitor visitor)
    {
        visitor.Execute(this);
    }
}