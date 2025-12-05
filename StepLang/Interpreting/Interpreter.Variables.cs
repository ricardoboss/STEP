using StepLang.Expressions.Results;
using StepLang.Parsing.Nodes.Statements;
using StepLang.Parsing.Nodes.VariableDeclarations;

namespace StepLang.Interpreting;

public partial class Interpreter
{
	public void Visit(VariableDeclarationStatementNode statementNode)
	{
		using var span = Telemetry.Profile(nameof(VariableDeclarationStatementNode));

		_ = statementNode.Declaration.EvaluateUsing(this);
	}

	public Variable Evaluate(VariableDeclarationNode expressionNode)
	{
		using var span = Telemetry.Profile(nameof(VariableDeclarationNode));

		var validResults = expressionNode.GetResultTypes().ToList();

		return CurrentScope.CreateVariable(expressionNode.Location, expressionNode.Identifier, validResults,
			ExpressionResult.DefaultFor(validResults.First()));
	}

	public Variable Evaluate(NullableVariableDeclarationNode expressionNode)
	{
		using var span = Telemetry.Profile(nameof(NullableVariableDeclarationNode));

		var validResults = expressionNode.GetResultTypes().ToList();

		return CurrentScope.CreateVariable(expressionNode.Location, expressionNode.Identifier, validResults,
			NullResult.Instance, true);
	}

	public Variable Evaluate(VariableInitializationNode expressionNode)
	{
		using var span = Telemetry.Profile(nameof(VariableInitializationNode));

		var validResults = expressionNode.GetResultTypes().ToList();

		return CurrentScope.CreateVariable(expressionNode.Location, expressionNode.Identifier, validResults,
			expressionNode.Expression.EvaluateUsing(this));
	}

	public Variable Evaluate(NullableVariableInitializationNode expressionNode)
	{
		using var span = Telemetry.Profile(nameof(NullableVariableInitializationNode));

		var validResults = expressionNode.GetResultTypes().ToList();

		return CurrentScope.CreateVariable(expressionNode.Location, expressionNode.Identifier, validResults,
			expressionNode.Expression.EvaluateUsing(this), true);
	}

	public Variable Evaluate(ErrorVariableDeclarationNode variableDeclarationNode)
	{
		using var span = Telemetry.Profile(nameof(ErrorVariableDeclarationNode));

		throw new NotSupportedException("Cannot evaluate error variable declaration node");
	}

	public void Visit(VariableAssignmentNode statementNode)
	{
		using var span = Telemetry.Profile(nameof(VariableAssignmentNode));

		var variable = CurrentScope.GetVariable(statementNode.Identifier);

		var result = statementNode.Expression.EvaluateUsing(this);

		variable.Assign(statementNode.Location, result);
	}
}
