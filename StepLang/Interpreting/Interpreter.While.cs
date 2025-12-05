using StepLang.Parsing.Nodes.Statements;

namespace StepLang.Interpreting;

public partial class Interpreter
{
	public void Visit(WhileStatementNode statementNode)
	{
		using var span = Telemetry.Profile(nameof(WhileStatementNode));

		while (ShouldLoop())
		{
			var loopScope = PushScope();

			statementNode.Body.Accept(this);

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
