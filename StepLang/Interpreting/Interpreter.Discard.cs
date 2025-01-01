using StepLang.Parsing.Nodes.Statements;

namespace StepLang.Interpreting;

public partial class Interpreter
{
	public void Visit(DiscardStatementNode discardStatementNode)
	{
		_ = discardStatementNode.Expression.EvaluateUsing(this);
	}
}
