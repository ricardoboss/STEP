using StepLang.Tokenizing;

namespace StepLang.Parsing;

public abstract record BinaryExpressionNode(
	TokenLocation OperatorLocation,
	ExpressionNode Left,
	ExpressionNode Right,
	BinaryExpressionOperator Op) : ExpressionNode, IBinaryExpressionNode
{
	public override TokenLocation Location => OperatorLocation;
}
