using StepLang.Tokenizing;

namespace StepLang.Parsing;

public sealed record ForeachDeclareKeyValueStatementNode(IVariableDeclarationNode KeyDeclaration, Token ValueIdentifier, ExpressionNode Collection, IReadOnlyCollection<StatementNode> Body) : StatementNode
{
    public override void Accept(IStatementVisitor visitor)
    {
        visitor.Execute(this);
    }
}