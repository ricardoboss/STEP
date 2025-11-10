using StepLang.Expressions.Results;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Nodes.Expressions;

public interface IExpressionNode : IEvaluatableNode<IExpressionEvaluator, ExpressionResult>
{
	Token FirstToken { get; }
}
