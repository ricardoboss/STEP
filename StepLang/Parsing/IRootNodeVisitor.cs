using StepLang.Parsing.Nodes;

namespace StepLang.Parsing;

public interface IRootNodeVisitor
{
	void Visit(RootNode node);
}
