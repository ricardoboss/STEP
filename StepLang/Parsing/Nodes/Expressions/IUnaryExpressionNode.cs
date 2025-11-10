namespace StepLang.Parsing.Nodes.Expressions;

public interface IUnaryExpressionNode : IExpressionNode
{
	IExpressionNode Expression { get; }
}
