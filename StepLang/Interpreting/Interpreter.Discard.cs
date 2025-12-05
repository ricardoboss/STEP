using StepLang.Parsing.Nodes.Statements;

namespace StepLang.Interpreting;

public partial class Interpreter
{
	public void Visit(DiscardStatementNode discardStatementNode)
	{
		using var span = Telemetry.Profile(nameof(DiscardStatementNode));

		_ = discardStatementNode.Expression.EvaluateUsing(this);
	}
}
