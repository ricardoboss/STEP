using StepLang.Tokenizing;

namespace StepLang.Parsing.Nodes;

public sealed record RootNode(IReadOnlyCollection<ImportNode> Imports, IReadOnlyCollection<StatementNode> Body)
	: IVisitableNode<IRootNodeVisitor>
{
	public void Accept(IRootNodeVisitor visitor)
	{
		visitor.Visit(this);
	}

	public TokenLocation Location =>
		Imports.FirstOrDefault()?.Location ?? Body.FirstOrDefault()?.Location ?? new TokenLocation();
}
