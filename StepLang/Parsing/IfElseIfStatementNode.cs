using StepLang.Tokenizing;

namespace StepLang.Parsing;

public sealed record IfElseIfStatementNode(Token IfKeywordToken, ExpressionNode Condition, CodeBlockStatementNode Body, ExpressionNode ElseCondition, CodeBlockStatementNode ElseBody) : StatementNode
{
    public override void Accept(IStatementVisitor visitor)
    {
        visitor.Visit(this);
    }

    public override TokenLocation Location => IfKeywordToken.Location;
}