using StepLang.Tokenizing;

namespace StepLang.Parsing;

public sealed record ForeachValueStatementNode(Token Identifier, ExpressionNode Collection, IReadOnlyCollection<StatementNode> Body) : StatementNode
{
    public override void Accept(IStatementVisitor visitor)
    {
        visitor.Execute(this);
    }
}