using StepLang.Expressions.Results;
using StepLang.Parsing.Nodes.Statements;
using StepLang.Parsing.Nodes.VariableDeclarations;
using StepLang.Tokenizing;

namespace StepLang.Interpreting;

public partial class Interpreter
{
	private void InitializeVariable(IVariableDeclarationNode declaration, ExpressionResult value)
	{
		var variable = declaration.EvaluateUsing(this);
		var location = declaration.Types.First().Location;

		variable.Assign(location, value);
	}

	public void Visit(ForeachDeclareKeyDeclareValueStatementNode statementNode)
	{
		using var span = Telemetry.Profile(nameof(ForeachDeclareKeyDeclareValueStatementNode));

		var collection = statementNode.Collection.EvaluateUsing(this);
		var collectionLocation = statementNode.Collection.Location;

		RunForeachLoop(
			e => InitializeVariable(statementNode.KeyDeclaration, e),
			e => InitializeVariable(statementNode.ValueDeclaration, e),
			collectionLocation,
			collection,
			statementNode.Body
		);
	}

	public void Visit(ForeachDeclareKeyValueStatementNode statementNode)
	{
		using var span = Telemetry.Profile(nameof(ForeachDeclareKeyValueStatementNode));

		var valueVariable = CurrentScope.GetVariable(statementNode.ValueIdentifier);
		var valueLocation = statementNode.ValueIdentifier.Location;
		var collection = statementNode.Collection.EvaluateUsing(this);
		var collectionLocation = statementNode.Collection.Location;

		RunForeachLoop(
			e => InitializeVariable(statementNode.KeyDeclaration, e),
			e => valueVariable.Assign(valueLocation, e),
			collectionLocation,
			collection,
			statementNode.Body
		);
	}

	public void Visit(ForeachDeclareValueStatementNode statementNode)
	{
		using var span = Telemetry.Profile(nameof(ForeachDeclareValueStatementNode));

		var collection = statementNode.Collection.EvaluateUsing(this);
		var collectionLocation = statementNode.Collection.Location;

		RunForeachLoop(
			null,
			e => InitializeVariable(statementNode.ValueDeclaration, e),
			collectionLocation,
			collection,
			statementNode.Body
		);
	}

	public void Visit(ForeachKeyValueStatementNode statementNode)
	{
		using var span = Telemetry.Profile(nameof(ForeachKeyValueStatementNode));

		var keyVariable = CurrentScope.GetVariable(statementNode.KeyIdentifier);
		var keyLocation = statementNode.KeyIdentifier.Location;
		var valueVariable = CurrentScope.GetVariable(statementNode.ValueIdentifier);
		var valueLocation = statementNode.ValueIdentifier.Location;
		var collection = statementNode.Collection.EvaluateUsing(this);
		var collectionLocation = statementNode.Collection.Location;

		RunForeachLoop(
			e => keyVariable.Assign(keyLocation, e),
			e => valueVariable.Assign(valueLocation, e),
			collectionLocation,
			collection,
			statementNode.Body
		);
	}

	public void Visit(ForeachKeyDeclareValueStatementNode statementNode)
	{
		using var span = Telemetry.Profile(nameof(ForeachKeyDeclareValueStatementNode));

		var collection = statementNode.Collection.EvaluateUsing(this);
		var collectionLocation = statementNode.Collection.Location;

		RunForeachLoop(
			null,
			e => InitializeVariable(statementNode.ValueDeclaration, e),
			collectionLocation,
			collection,
			statementNode.Body
		);
	}

	public void Visit(ForeachValueStatementNode statementNode)
	{
		using var span = Telemetry.Profile(nameof(ForeachValueStatementNode));

		var valueVariable = CurrentScope.GetVariable(statementNode.Identifier);
		var valueLocation = statementNode.Identifier.Location;
		var collection = statementNode.Collection.EvaluateUsing(this);
		var collectionLocation = statementNode.Collection.Location;

		RunForeachLoop(
			null,
			e => valueVariable.Assign(valueLocation, e),
			collectionLocation,
			collection,
			statementNode.Body
		);
	}

	private static IEnumerable<(ExpressionResult, ExpressionResult)> ConvertToForeachEnumerable(
		TokenLocation evaluationLocation, ExpressionResult collection)
	{
		switch (collection)
		{
			case ListResult { Value: var items }:
				{
					var index = 0;
					foreach (var item in items)
					{
						yield return (new NumberResult(index), item);

						index++;
					}

					yield break;
				}
			case MapResult { Value: var pairs }:
				{
					foreach (var (key, value) in pairs)
					{
						yield return (new StringResult(key), value);
					}

					yield break;
				}
			default:
				throw new InvalidResultTypeException(evaluationLocation, collection, ResultType.List, ResultType.Map);
		}
	}

	private void RunForeachLoop(Action<ExpressionResult>? updateKey, Action<ExpressionResult> updateValue,
		TokenLocation collectionLocation, ExpressionResult collection, StatementNode body)
	{
		var pairs = ConvertToForeachEnumerable(collectionLocation, collection);

		foreach (var (keyValue, valueValue) in pairs)
		{
			var loopScope = PushScope();

			// update/initialize key and value in loop scope
			updateKey?.Invoke(keyValue);
			updateValue(valueValue);

			body.Accept(this);

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
	}
}
