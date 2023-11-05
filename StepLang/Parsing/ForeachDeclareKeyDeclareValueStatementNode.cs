namespace StepLang.Parsing;

public sealed record ForeachDeclareKeyDeclareValueStatementNode(IVariableDeclarationNode KeyDeclaration, IVariableDeclarationNode ValueDeclaration, ExpressionNode Collection, IReadOnlyCollection<StatementNode> Body) : StatementNode
{
    public override void Accept(IStatementVisitor visitor)
    {
        visitor.Execute(this);
    }
}