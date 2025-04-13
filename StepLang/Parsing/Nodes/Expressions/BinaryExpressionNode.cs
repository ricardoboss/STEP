using StepLang.Tokenizing;

namespace StepLang.Parsing.Nodes.Expressions;

public abstract record BinaryExpressionNode(
	Token Operator,
	ExpressionNode Left,
	ExpressionNode Right,
	BinaryExpressionOperator Op) : ExpressionNode, IBinaryExpressionNode
{
	public override Token? FirstToken => Operator;

	public override TokenLocation Location => Operator.Location;
}
