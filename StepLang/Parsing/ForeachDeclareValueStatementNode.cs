using StepLang.Tokenizing;

namespace StepLang.Parsing;

public sealed record ForeachDeclareValueStatementNode(IVariableDeclarationNode ValueDeclaration, ExpressionNode Collection, IReadOnlyCollection<StatementNode> Body) : StatementNode
{
    public override void Accept(IStatementVisitor visitor)
    {
        visitor.Execute(this);
    }
}