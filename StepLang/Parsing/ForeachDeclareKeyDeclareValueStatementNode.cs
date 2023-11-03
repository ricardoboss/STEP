using StepLang.Tokenizing;

namespace StepLang.Parsing;

public sealed record ForeachDeclareKeyDeclareValueStatementNode(Token KeyType, Token KeyIdentifier, Token ValueType, Token ValueIdentifier, ExpressionNode List, IReadOnlyCollection<StatementNode> Body) : StatementNode
{
    public override void Accept(IStatementVisitor visitor)
    {
        visitor.Execute(this);
    }
}