using StepLang.Tokenizing;

namespace StepLang.Parsing;

public interface INode
{
	TokenLocation Location { get; }
}
