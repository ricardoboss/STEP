using StepLang.Tokenizing;

namespace StepLang.Parsing;

public sealed record IfStatementNode(Token IfKeywordToken, ExpressionNode Condition, IReadOnlyCollection<StatementNode> Body) : StatementNode
{
    public override void Accept(IStatementVisitor visitor)
    {
        visitor.Execute(this);
    }

    public override TokenLocation Location => IfKeywordToken.Location;
}