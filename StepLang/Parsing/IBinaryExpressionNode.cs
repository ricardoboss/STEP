using StepLang.Tokenizing;

namespace StepLang.Parsing;

public interface IBinaryExpressionNode
{
	ExpressionNode Left { get; }
	ExpressionNode Right { get; }
	BinaryExpressionOperator Op { get; }
	TokenLocation OperatorLocation { get; }
}
