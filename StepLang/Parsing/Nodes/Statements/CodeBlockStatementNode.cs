using StepLang.Tokenizing;

namespace StepLang.Parsing.Nodes.Statements;

public sealed record CodeBlockStatementNode(Token OpenCurlyBraceToken, IReadOnlyList<StatementNode> Statements, Token CloseCurlyBraceToken)
	: StatementNode
{
	public override void Accept(IStatementVisitor visitor)
	{
		visitor.Visit(this);
	}

	public override TokenLocation Location => OpenCurlyBraceToken.Location;
}
