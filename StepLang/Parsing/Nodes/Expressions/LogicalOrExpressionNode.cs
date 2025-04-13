using StepLang.Expressions.Results;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Nodes.Expressions;

public record LogicalOrExpressionNode(Token Operator, ExpressionNode Left, ExpressionNode Right)
	: BinaryExpressionNode(Operator, Left, Right, BinaryExpressionOperator.LogicalOr)
{
	public override ExpressionResult EvaluateUsing(IExpressionEvaluator evaluator)
	{
		return evaluator.Evaluate(this);
	}
}
