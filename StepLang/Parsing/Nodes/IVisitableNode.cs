namespace StepLang.Parsing.Nodes;

public interface IVisitableNode<in TVisitor> : INode
{
	void Accept(TVisitor visitor);
}
