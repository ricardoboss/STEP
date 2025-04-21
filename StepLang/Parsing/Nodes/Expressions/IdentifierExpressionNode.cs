using StepLang.Expressions.Results;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Nodes.Expressions;

public sealed record IdentifierExpressionNode(IReadOnlyList<Token> IdentifierChain) : ExpressionNode
{
	public override ExpressionResult EvaluateUsing(IExpressionEvaluator evaluator)
	{
		return evaluator.Evaluate(this);
	}

	public override Token FirstToken => IdentifierChain[0];

	public override TokenLocation Location => FirstToken.Location with
	{
		// treat whole chain as location
		Length = IdentifierChain.Aggregate(0, (s, t) => s + t.Location.Length) + IdentifierChain.Count - 1,
	};
}
