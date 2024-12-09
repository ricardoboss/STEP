using StepLang.Expressions.Results;
using StepLang.Tokenizing;

namespace StepLang.Parsing;

public record GreaterThanExpressionNode(TokenLocation OperatorLocation, ExpressionNode Left, ExpressionNode Right)
	: BinaryExpressionNode(OperatorLocation, Left, Right, BinaryExpressionOperator.GreaterThan)
{
	public override ExpressionResult EvaluateUsing(IExpressionEvaluator evaluator)
	{
		return evaluator.Evaluate(this);
	}
}
