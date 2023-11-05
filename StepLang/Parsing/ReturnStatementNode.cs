using StepLang.Tokenizing;

namespace StepLang.Parsing;

public sealed record ReturnStatementNode(Token ReturnKeywordToken, ExpressionNode Expression) : StatementNode
{
    public override void Accept(IStatementVisitor visitor)
    {
        visitor.Execute(this);
    }

    public override TokenLocation Location => ReturnKeywordToken.Location;
}