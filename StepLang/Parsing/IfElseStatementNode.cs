using StepLang.Tokenizing;

namespace StepLang.Parsing;

public sealed record IfElseStatementNode(Token IfKeywordToken, ExpressionNode Condition, IReadOnlyCollection<StatementNode> Body, IReadOnlyCollection<StatementNode> ElseBody) : StatementNode
{
    public override void Accept(IStatementVisitor visitor)
    {
        visitor.Visit(this);
    }

    public override TokenLocation Location => IfKeywordToken.Location;
}