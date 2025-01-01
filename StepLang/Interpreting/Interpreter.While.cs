using StepLang.Parsing.Nodes;

namespace StepLang.Interpreting;

public partial class Interpreter
{
	public void Visit(WhileStatementNode statementNode)
	{
		while (ShouldLoop())
		{
			var loopScope = PushScope();

			Execute(statementNode.Body);

			_ = PopScope();

			// handle returns to parent scope
			if (loopScope.TryGetResult(out var resultValue, out var resultLocation))
			{
				CurrentScope.SetResult(resultLocation, resultValue);

				return;
			}

			// break out of loop
			if (loopScope.ShouldBreak())
			{
				return;
			}

			// continue is implicitly handled
		}

		return;

		bool ShouldLoop()
		{
			var result = statementNode.Condition.EvaluateUsing(this);

			return result.IsTruthy();
		}
	}
}
