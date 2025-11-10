using StepLang.Interpreting;
using StepLang.Parsing.Nodes.Expressions;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Nodes.VariableDeclarations;

public sealed record VariableInitializationNode(
	TokenLocation AssignmentLocation,
	IEnumerable<Token> Types,
	Token Identifier,
	IExpressionNode Expression) : IVariableInitializationNode
{
	public Variable EvaluateUsing(IVariableDeclarationEvaluator evaluator)
	{
		return evaluator.Evaluate(this);
	}

	public TokenLocation Location => AssignmentLocation;

	public bool HasValue => true;
}
