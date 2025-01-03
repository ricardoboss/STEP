using StepLang.Parsing.Nodes.Expressions;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Nodes.Statements;

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