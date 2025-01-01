using StepLang.Expressions.Results;
using StepLang.Framework;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Nodes;

public record NativeFunctionDefinitionExpressionNode(NativeFunction Definition) : ExpressionNode
{
	public override ExpressionResult EvaluateUsing(IExpressionEvaluator evaluator)
	{
		return evaluator.Evaluate(this);
	}

	public override TokenLocation Location { get; } = new();
}
