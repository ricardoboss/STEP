using StepLang.Expressions.Results;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Nodes.Expressions;

public record NotExpressionNode(Token ExclamationMarkToken, ExpressionNode Expression)
	: ExpressionNode, IUnaryExpressionNode
{
	public override ExpressionResult EvaluateUsing(IExpressionEvaluator evaluator)
	{
		return evaluator.Evaluate(this);
	}

	public override TokenLocation Location => ExclamationMarkToken.Location;
}
