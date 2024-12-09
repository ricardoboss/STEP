using StepLang.Expressions.Results;
using StepLang.Parsing;

namespace StepLang.Interpreting;

public partial class Interpreter : IVariableDeclarationEvaluator
{
	public void Visit(VariableDeclarationStatementNode statementNode)
	{
		_ = statementNode.Declaration.EvaluateUsing(this);
	}

	public Variable Evaluate(VariableDeclarationNode statementNode)
	{
		var validResults = statementNode.GetResultTypes().ToList();

		return CurrentScope.CreateVariable(statementNode.Location, statementNode.Identifier, validResults,
			ExpressionResult.DefaultFor(validResults.First()), false);
	}

	public Variable Evaluate(NullableVariableDeclarationNode statementNode)
	{
		var validResults = statementNode.GetResultTypes().ToList();

		return CurrentScope.CreateVariable(statementNode.Location, statementNode.Identifier, validResults,
			NullResult.Instance, true);
	}

	public Variable Evaluate(VariableInitializationNode statementNode)
	{
		var validResults = statementNode.GetResultTypes().ToList();

		return CurrentScope.CreateVariable(statementNode.Location, statementNode.Identifier, validResults,
			statementNode.Expression.EvaluateUsing(this), false);
	}

	public Variable Evaluate(NullableVariableInitializationNode statementNode)
	{
		var validResults = statementNode.GetResultTypes().ToList();

		return CurrentScope.CreateVariable(statementNode.Location, statementNode.Identifier, validResults,
			statementNode.Expression.EvaluateUsing(this), true);
	}

	public void Visit(VariableAssignmentNode statementNode)
	{
		var variable = CurrentScope.GetVariable(statementNode.Identifier);

		var result = statementNode.Expression.EvaluateUsing(this);

		variable.Assign(statementNode.Location, result);
	}
}
