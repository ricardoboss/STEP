using StepLang.Expressions.Results;
using StepLang.Parsing;

namespace StepLang.Interpreting;

public partial class Interpreter
{
	public void Visit(ReturnExpressionStatementNode statementNode)
	{
		var result = statementNode.Expression.EvaluateUsing(this);

		CurrentScope.SetResult(statementNode.Location, result);
	}

	public void Visit(ReturnStatementNode statementNode)
	{
		CurrentScope.SetResult(statementNode.Location, VoidResult.Instance);
	}
}
