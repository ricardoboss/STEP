namespace StepLang.Parsing;

public sealed record ReturnStatementNode(ExpressionNode Expression) : StatementNode
{
    public override void Accept(IStatementVisitor visitor)
    {
        visitor.Execute(this);
    }
}