using StepLang.Expressions.Results;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Nodes.Expressions;

public sealed record ListExpressionNode(Token OpenBracketToken, IReadOnlyCollection<ExpressionNode> Expressions)
	: ExpressionNode
{
	public override ExpressionResult EvaluateUsing(IExpressionEvaluator evaluator)
	{
		return evaluator.Evaluate(this);
	}

	public override Token FirstToken => OpenBracketToken;
}
