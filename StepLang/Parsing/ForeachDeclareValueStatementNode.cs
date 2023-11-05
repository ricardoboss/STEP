namespace StepLang.Parsing;

public sealed record ForeachDeclareValueStatementNode(Token ForeachKeywordToken, IVariableDeclarationNode ValueDeclaration, ExpressionNode Collection, IReadOnlyCollection<StatementNode> Body) : StatementNode
{
    public override void Accept(IStatementVisitor visitor)
    {
        visitor.Execute(this);
    }

    public override TokenLocation Location => ForeachKeywordToken.Location;
}