using StepLang.Tokenizing;

namespace StepLang.Parsing;

public sealed record ForeachKeyValueStatementNode(Token ForeachKeywordToken, Token KeyIdentifier, Token ValueIdentifier, ExpressionNode Collection, IReadOnlyCollection<StatementNode> Body) : StatementNode
{
    public override void Accept(IStatementVisitor visitor)
    {
        visitor.Visit(this);
    }

    public override TokenLocation Location => ForeachKeywordToken.Location;
}