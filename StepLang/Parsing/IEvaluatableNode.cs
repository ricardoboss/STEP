namespace StepLang.Parsing;

public interface IEvaluatableNode<in TVisitor, out TResult> : INode
{
	TResult EvaluateUsing(TVisitor evaluator);
}
