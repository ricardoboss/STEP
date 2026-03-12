using StepLang.Expressions.Results;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Nodes.Expressions;

public abstract record ExpressionNode : IExpressionNode
{
	public abstract Token FirstToken { get; }

	public virtual TokenLocation Location => FirstToken.Location;

	public abstract ExpressionResult EvaluateUsing(IExpressionEvaluator evaluator);
}
