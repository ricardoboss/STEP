namespace StepLang.Parsing;

public interface IVisitableNode<in TVisitor> : INode
{
	void Accept(TVisitor visitor);
}
