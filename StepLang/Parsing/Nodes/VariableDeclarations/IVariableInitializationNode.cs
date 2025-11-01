using StepLang.Parsing.Nodes.Expressions;

namespace StepLang.Parsing.Nodes.VariableDeclarations;

public interface IVariableInitializationNode : IVariableDeclarationNode
{
	ExpressionNode Expression { get; }
}
