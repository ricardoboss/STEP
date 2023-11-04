namespace StepLang.Parsing;

public interface INode
{
    string Name => GetType().Name;
}
