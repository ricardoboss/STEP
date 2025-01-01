using StepLang.Tokenizing;

namespace StepLang.Parsing.Nodes.Expressions;

public abstract record BinaryExpressionNode(
	TokenLocation OperatorLocation,
	ExpressionNode Left,
	ExpressionNode Right,
	BinaryExpressionOperator Op) : ExpressionNode, IBinaryExpressionNode
{
	public override TokenLocation Location => OperatorLocation;
}
