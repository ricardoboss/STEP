using StepLang.Tokenizing;

namespace StepLang.Parsing.Nodes;

public record DecrementStatementNode(Token Identifier) : StatementNode
{
	public override void Accept(IStatementVisitor visitor)
	{
		visitor.Visit(this);
	}

	public override TokenLocation Location => Identifier.Location;
}
