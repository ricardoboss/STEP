using StepLang.Parsing.Nodes.Statements;

namespace StepLang.Interpreting;

public partial class Interpreter
{
	public void Visit(IfStatementNode statementNode)
	{
		using var span = Telemetry.Profile(nameof(IfStatementNode));

		foreach (var (condition, body) in statementNode.ConditionBodyMap)
		{
			var result = condition.EvaluateUsing(this);
			if (!result.IsTruthy())
			{
				continue;
			}

			body.Accept(this);

			break;
		}
	}

	public void Visit(ContinueStatementNode statementNode)
	{
		using var span = Telemetry.Profile(nameof(ContinueStatementNode));

		CurrentScope.SetContinue();
	}

	public void Visit(BreakStatementNode statementNode)
	{
		using var span = Telemetry.Profile(nameof(BreakStatementNode));

		CurrentScope.SetBreak();
	}
}
