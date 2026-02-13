using StepLang.Expressions.Results;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Nodes.Expressions;

public sealed record CallExpressionNode(Token Identifier, IReadOnlyList<IExpressionNode> Arguments) : IExpressionNode
{
	public ExpressionResult EvaluateUsing(IExpressionEvaluator evaluator)
	{
		return evaluator.Evaluate(this);
	}

	public Token FirstToken => Identifier;

	public TokenLocation Location => Identifier.Location;
}
