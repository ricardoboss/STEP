using StepLang.Expressions.Results;
using StepLang.Tokenizing;

namespace StepLang.Parsing;

public record ModuloExpressionNode(TokenLocation OperatorLocation, ExpressionNode Left, ExpressionNode Right)
	: BinaryExpressionNode(OperatorLocation, Left, Right, BinaryExpressionOperator.Modulo)
{
	public override ExpressionResult EvaluateUsing(IExpressionEvaluator evaluator)
	{
		return evaluator.Evaluate(this);
	}
}
