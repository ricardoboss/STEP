using StepLang.Tokenizing;

namespace StepLang.Parsing;

public interface INode
{
    string Name => GetType().Name;

    TokenLocation Location { get; }
}
