using StepLang.Tokenizing;

namespace StepLang.Parsing.Nodes.Statements;

public abstract record StatementNode : IVisitableNode<IStatementVisitor>
{
	public abstract void Accept(IStatementVisitor visitor);

	public abstract TokenLocation Location { get; }
}
