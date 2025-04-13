using StepLang.Expressions.Results;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Nodes.Expressions;

public abstract record ExpressionNode : IEvaluatableNode<IExpressionEvaluator, ExpressionResult>
{
	public abstract ExpressionResult EvaluateUsing(IExpressionEvaluator evaluator);

	public abstract Token FirstToken { get; }

	public virtual TokenLocation Location => FirstToken.Location;
}
