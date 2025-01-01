using StepLang.Interpreting;
using StepLang.Parsing.Nodes.Expressions;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Nodes.VariableDeclarations;

public sealed record NullableVariableInitializationNode(
	TokenLocation AssignmentLocation,
	IEnumerable<Token> Types,
	Token NullabilityIndicator,
	Token Identifier,
	ExpressionNode Expression) : IVariableDeclarationNode
{
	public Variable EvaluateUsing(IVariableDeclarationEvaluator evaluator)
	{
		return evaluator.Evaluate(this);
	}

	public TokenLocation Location => AssignmentLocation;

	public bool HasValue => true;
}
