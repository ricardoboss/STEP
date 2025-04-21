using StepLang.Tokenizing;

namespace StepLang.Parsing.Nodes.Statements;

public record DecrementStatementNode(IReadOnlyList<Token> IdentifierChain) : StatementNode
{
	public override void Accept(IStatementVisitor visitor)
	{
		visitor.Visit(this);
	}

	public override TokenLocation Location => IdentifierChain[0].Location;
}
