using StepLang.Tokenizing;

namespace StepLang.Parsing.Nodes;

public abstract record StatementNode : IVisitableNode<IStatementVisitor>
{
	public abstract void Accept(IStatementVisitor visitor);

	public abstract TokenLocation Location { get; }
}
