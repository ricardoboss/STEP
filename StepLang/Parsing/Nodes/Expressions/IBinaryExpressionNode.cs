using StepLang.Tokenizing;

namespace StepLang.Parsing.Nodes.Expressions;

public interface IBinaryExpressionNode : IExpressionNode
{
	IExpressionNode Left { get; }

	IExpressionNode Right { get; }

	BinaryExpressionOperator Op { get; }

	Token OperatorToken { get; }
}
