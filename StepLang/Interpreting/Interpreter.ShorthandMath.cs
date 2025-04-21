using StepLang.Expressions.Results;
using StepLang.Parsing.Nodes.Statements;

namespace StepLang.Interpreting;

public partial class Interpreter
{
	public void Visit(IncrementStatementNode statementNode)
	{
		var variable = CurrentScope.GetVariable(statementNode.IdentifierChain[0]);
		var value = variable.Value;
		if (value is not NumberResult number)
		{
			throw new IncompatibleExpressionOperandsException(statementNode.Location, value, "increment");
		}

		variable.Assign(statementNode.Location, number + 1);
	}

	public void Visit(DecrementStatementNode statementNode)
	{
		var variable = CurrentScope.GetVariable(statementNode.IdentifierChain[0]);
		var value = variable.Value;
		if (value is not NumberResult number)
		{
			throw new IncompatibleExpressionOperandsException(statementNode.Location, value, "decrement");
		}

		variable.Assign(statementNode.Location, number - 1);
	}
}
