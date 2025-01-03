using StepLang.Parsing.Nodes.VariableDeclarations;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Nodes.Statements;

public record VariableDeclarationStatementNode(IVariableDeclarationNode Declaration) : StatementNode
{
	public override void Accept(IStatementVisitor visitor)
	{
		visitor.Visit(this);
	}

	public override TokenLocation Location => Declaration.Location;
}
