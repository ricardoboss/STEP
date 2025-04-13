using StepLang.Expressions.Results;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Nodes.Expressions;

public record BitwiseShiftLeftExpressionNode(Token Operator, ExpressionNode Left, ExpressionNode Right)
	: BinaryExpressionNode(Operator, Left, Right, BinaryExpressionOperator.BitwiseShiftLeft)
{
	public override ExpressionResult EvaluateUsing(IExpressionEvaluator evaluator)
	{
		return evaluator.Evaluate(this);
	}
}
