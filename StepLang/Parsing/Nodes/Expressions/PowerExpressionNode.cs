using StepLang.Expressions.Results;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Nodes.Expressions;

public record PowerExpressionNode(Token Operator, ExpressionNode Left, ExpressionNode Right)
	: BinaryExpressionNode(Operator, Left, Right, BinaryExpressionOperator.Power)
{
	public override ExpressionResult EvaluateUsing(IExpressionEvaluator evaluator)
	{
		return evaluator.Evaluate(this);
	}
}
