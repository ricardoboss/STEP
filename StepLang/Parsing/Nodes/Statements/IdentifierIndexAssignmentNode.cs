using StepLang.Parsing.Nodes.Expressions;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Nodes.Statements;

public sealed record IdentifierIndexAssignmentNode(
	Token Identifier,
	IReadOnlyList<IExpressionNode> IndexExpressions,
	Token AssignmentToken,
	IExpressionNode ValueExpression) : StatementNode
{
	public override void Accept(IStatementVisitor visitor)
	{
		visitor.Visit(this);
	}

	public override TokenLocation Location => Identifier.Location;
}
