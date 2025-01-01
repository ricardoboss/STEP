using StepLang.Tokenizing;

namespace StepLang.Parsing.Nodes;

public record ReturnStatementNode(Token ReturnKeyword) : StatementNode
{
	public override void Accept(IStatementVisitor visitor)
	{
		visitor.Visit(this);
	}

	public override TokenLocation Location => ReturnKeyword.Location;
}
