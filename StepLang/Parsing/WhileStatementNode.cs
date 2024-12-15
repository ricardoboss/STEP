using StepLang.Tokenizing;

namespace StepLang.Parsing;

public sealed record WhileStatementNode(
	Token WhileKeywordToken,
	ExpressionNode Condition,
	CodeBlockStatementNode Body) : StatementNode
{
	public override void Accept(IStatementVisitor visitor)
	{
		visitor.Visit(this);
	}

	public override TokenLocation Location => WhileKeywordToken.Location;
}
