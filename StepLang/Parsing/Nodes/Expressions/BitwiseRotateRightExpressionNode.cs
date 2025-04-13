using StepLang.Expressions.Results;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Nodes.Expressions;

public record BitwiseRotateRightExpressionNode(
	Token Operator,
	ExpressionNode Left,
	ExpressionNode Right)
	: BinaryExpressionNode(Operator, Left, Right, BinaryExpressionOperator.BitwiseRotateRight)
{
	public override ExpressionResult EvaluateUsing(IExpressionEvaluator evaluator)
	{
		return evaluator.Evaluate(this);
	}
}
