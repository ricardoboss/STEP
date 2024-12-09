using StepLang.Expressions.Results;
using StepLang.Tokenizing;

namespace StepLang.Parsing;

public sealed record IdentifierExpressionNode(Token Identifier) : ExpressionNode
{
	public override ExpressionResult EvaluateUsing(IExpressionEvaluator evaluator)
	{
		return evaluator.Evaluate(this);
	}

	public override TokenLocation Location => Identifier.Location;
}
