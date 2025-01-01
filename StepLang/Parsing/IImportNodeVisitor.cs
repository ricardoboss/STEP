using StepLang.Parsing.Nodes;

namespace StepLang.Parsing;

public interface IImportNodeVisitor
{
	void Visit(ImportNode importNode);
}
