namespace StepLang.Parsing;

public interface IBinaryExpressionNode
{
    ExpressionNode Left { get; }
    ExpressionNode Right { get; }
    BinaryExpressionOperator Operator { get; }
}