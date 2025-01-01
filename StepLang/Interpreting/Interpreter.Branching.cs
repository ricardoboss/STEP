using StepLang.Parsing.Nodes.Statements;

namespace StepLang.Interpreting;

public partial class Interpreter
{
	public void Visit(IfStatementNode statementNode)
	{
		foreach (var (condition, body) in statementNode.ConditionBodyMap)
		{
			var result = condition.EvaluateUsing(this);
			if (!result.IsTruthy())
			{
				continue;
			}

			Visit(body);

			break;
		}
	}

	public void Visit(ContinueStatementNode statementNode)
	{
		CurrentScope.SetContinue();
	}

	public void Visit(BreakStatementNode statementNode)
	{
		CurrentScope.SetBreak();
	}
}
