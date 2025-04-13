using StepLang.Tokenizing;

namespace StepLang.Parsing.Nodes.Expressions;

public interface IBinaryExpressionNode
{
	ExpressionNode Left { get; }
	ExpressionNode Right { get; }
	BinaryExpressionOperator Op { get; }
	Token OperatorToken { get; }
}
