using StepLang.Parsing.Nodes.Expressions;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Nodes.Statements;

public sealed record IdentifierIndexAssignmentNode(
	IReadOnlyList<Token> IdentifierChain,
	IReadOnlyList<ExpressionNode> IndexExpressions,
	Token AssignmentToken,
	ExpressionNode ValueExpression) : StatementNode
{
	public override void Accept(IStatementVisitor visitor)
	{
		visitor.Visit(this);
	}

	public override TokenLocation Location => IdentifierChain[0].Location;
}
