using StepLang.Tokenizing;

namespace StepLang.Parsing.Nodes.Expressions;

public abstract record BinaryExpressionNode(
	Token OperatorToken,
	ExpressionNode Left,
	ExpressionNode Right,
	BinaryExpressionOperator Op) : ExpressionNode, IBinaryExpressionNode
{
	public override Token FirstToken => OperatorToken;

	public override TokenLocation Location => OperatorToken.Location;
}
