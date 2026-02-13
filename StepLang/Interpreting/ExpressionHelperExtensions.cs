using StepLang.Expressions.Results;
using StepLang.Parsing.Nodes.Expressions;

namespace StepLang.Interpreting;

public static class ExpressionHelperExtensions
{
	public static IEnumerable<ExpressionResult> EvaluateUsing(this IEnumerable<IExpressionNode> nodes,
		IInterpreter interpreter)
	{
		return nodes.Select(node => node.EvaluateUsing(interpreter));
	}
}
