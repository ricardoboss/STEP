using StepLang.Tokenizing;

namespace StepLang.Parsing;

public sealed record ReturnExpressionStatementNode(Token ReturnKeywordToken, ExpressionNode Expression) : StatementNode
{
    public override void Accept(IStatementVisitor visitor)
    {
        visitor.Execute(this);
    }

    public override TokenLocation Location => ReturnKeywordToken.Location;
}