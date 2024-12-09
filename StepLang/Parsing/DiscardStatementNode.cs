using StepLang.Tokenizing;

namespace StepLang.Parsing;

public sealed record DiscardStatementNode(TokenLocation UnderscoreTokenLocation, ExpressionNode Expression)
	: StatementNode
{
	public override void Accept(IStatementVisitor visitor)
	{
		visitor.Visit(this);
	}

	public override TokenLocation Location { get; } = UnderscoreTokenLocation;
}
