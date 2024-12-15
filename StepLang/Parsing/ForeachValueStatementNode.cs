using StepLang.Tokenizing;

namespace StepLang.Parsing;

public sealed record ForeachValueStatementNode(
	Token ForeachKeywordToken,
	Token Identifier,
	ExpressionNode Collection,
	CodeBlockStatementNode Body) : StatementNode
{
	public override void Accept(IStatementVisitor visitor)
	{
		visitor.Visit(this);
	}

	public override TokenLocation Location => ForeachKeywordToken.Location;
}
