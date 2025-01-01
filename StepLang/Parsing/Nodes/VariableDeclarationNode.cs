using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Nodes;

public sealed record VariableDeclarationNode(IEnumerable<Token> Types, Token Identifier) : IVariableDeclarationNode
{
	public Variable EvaluateUsing(IVariableDeclarationEvaluator evaluator)
	{
		return evaluator.Evaluate(this);
	}

	public TokenLocation Location => Types.First().Location;

	public bool HasValue => false;
}
