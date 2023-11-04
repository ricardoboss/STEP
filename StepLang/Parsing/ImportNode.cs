using StepLang.Tokenizing;

namespace StepLang.Parsing;

public sealed record ImportNode(Token PathToken) : IVisitableNode<IImportNodeVisitor>
{
    public void Accept(IImportNodeVisitor visitor)
    {
        visitor.Visit(this);
    }
}