using StepLang.Parsing.Nodes.Expressions;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Nodes.Statements;

public sealed record VariableAssignmentNode(
	TokenLocation AssignmentLocation,
	IReadOnlyList<Token> IdentifierChain,
	ExpressionNode Expression) : StatementNode
{
	public override void Accept(IStatementVisitor visitor)
	{
		visitor.Visit(this);
	}

	public override TokenLocation Location => AssignmentLocation;
}
