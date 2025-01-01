using StepLang.Tokenizing;

namespace StepLang.Parsing.Nodes.Statements;

public sealed record BreakStatementNode(Token Token) : StatementNode
{
	public override void Accept(IStatementVisitor visitor)
	{
		visitor.Visit(this);
	}

	public override TokenLocation Location => Token.Location;
}
