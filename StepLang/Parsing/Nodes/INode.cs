using StepLang.Tokenizing;

namespace StepLang.Parsing.Nodes;

public interface INode
{
	TokenLocation Location { get; }
}
