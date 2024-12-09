using StepLang.Expressions.Results;
using StepLang.Tokenizing;

namespace StepLang.Parsing;

public record IndexAccessExpressionNode(ExpressionNode Left, ExpressionNode Index) : ExpressionNode
{
	public override ExpressionResult EvaluateUsing(IExpressionEvaluator evaluator)
	{
		return evaluator.Evaluate(this);
	}

	public override TokenLocation Location => Index.Location;
}
