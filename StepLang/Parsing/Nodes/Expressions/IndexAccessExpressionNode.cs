using StepLang.Expressions.Results;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Nodes.Expressions;

public record IndexAccessExpressionNode(Token OpenBracket, ExpressionNode Left, ExpressionNode Index) : ExpressionNode
{
	public override ExpressionResult EvaluateUsing(IExpressionEvaluator evaluator)
	{
		return evaluator.Evaluate(this);
	}

	public override Token FirstToken => OpenBracket;
}
