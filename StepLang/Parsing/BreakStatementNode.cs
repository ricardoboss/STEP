using StepLang.Tokenizing;

namespace StepLang.Parsing;

public sealed record BreakStatementNode(Token Token) : StatementNode
{
	public override void Accept(IStatementVisitor visitor)
	{
		visitor.Visit(this);
	}

	public override TokenLocation Location => Token.Location;
}
