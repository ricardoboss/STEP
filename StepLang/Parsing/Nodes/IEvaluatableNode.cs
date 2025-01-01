namespace StepLang.Parsing.Nodes;

public interface IEvaluatableNode<in TVisitor, out TResult> : INode
{
	TResult EvaluateUsing(TVisitor evaluator);
}
