using StepLang.Tokenizing;

namespace StepLang.Parsing.Nodes;

public sealed record ImportNode(Token PathToken) : IVisitableNode<IImportNodeVisitor>
{
	public void Accept(IImportNodeVisitor visitor)
	{
		visitor.Visit(this);
	}

	public TokenLocation Location => PathToken.Location;
}
