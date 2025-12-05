using StepLang.Expressions.Results;
using StepLang.Parsing.Nodes.Statements;

namespace StepLang.Interpreting;

public partial class Interpreter
{
	public void Visit(ReturnExpressionStatementNode statementNode)
	{
		using var span = Telemetry.Profile(nameof(ReturnExpressionStatementNode));

		var result = statementNode.Expression.EvaluateUsing(this);

		CurrentScope.SetResult(statementNode.Location, result);
	}

	public void Visit(ReturnStatementNode statementNode)
	{
		using var span = Telemetry.Profile(nameof(ReturnStatementNode));

		CurrentScope.SetResult(statementNode.Location, VoidResult.Instance);
	}
}
