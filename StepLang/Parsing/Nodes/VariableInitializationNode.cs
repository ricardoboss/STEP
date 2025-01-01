using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Nodes;

public sealed record VariableInitializationNode(
	TokenLocation AssignmentLocation,
	IEnumerable<Token> Types,
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
