using StepLang.Tokenizing;

namespace StepLang.Parsing;

public sealed record IfElseIfStatementNode(Token IfKeywordToken, ExpressionNode Condition, IReadOnlyCollection<StatementNode> Body, ExpressionNode ElseCondition, IReadOnlyCollection<StatementNode> ElseBody) : StatementNode
{
    public override void Accept(IStatementVisitor visitor)
    {
        visitor.Execute(this);
    }

    public override TokenLocation Location => IfKeywordToken.Location;
}