using StepLang.Tokenizing;

namespace StepLang.Parsing.Nodes;

public sealed record ReturnExpressionStatementNode(Token ReturnKeywordToken, ExpressionNode Expression) : StatementNode
{
	public override void Accept(IStatementVisitor visitor)
	{
		visitor.Visit(this);
	}

	public override TokenLocation Location => ReturnKeywordToken.Location;
}
