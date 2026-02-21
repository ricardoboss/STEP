using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing.Nodes.Expressions;
using StepLang.Parsing.Nodes.VariableDeclarations;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

public class SortedFunction : ListManipulationFunction
{
	public const string Identifier = "sorted";

	public override ExpressionResult Invoke(TokenLocation callLocation, IInterpreter interpreter,
		IReadOnlyList<IExpressionNode> arguments)
	{
		CheckArgumentCount(callLocation, arguments, 1, 2);

		if (arguments.Count == 2)
		{
			return base.Invoke(callLocation, interpreter, arguments);
		}

		return base.Invoke(callLocation, interpreter,
			[arguments[0], new CompareToFunction().ToResult().ToExpressionNode()]);
	}

	protected override IEnumerable<IExpressionNode[]> PrepareArgsForCallback(TokenLocation callLocation,
		IEnumerable<ExpressionResult> list, FunctionDefinition callback)
	{
		var callbackParameters = callback.Parameters.ToList();
		if (callbackParameters.Count != 2)
		{
			throw new InvalidArgumentTypeException(callLocation,
				$"Callback function must have 2 parameters, but has {callbackParameters.Count}");
		}

		if (!callbackParameters[0].GetResultTypes().SequenceEqual(callbackParameters[1].GetResultTypes()))
		{
			throw new InvalidArgumentTypeException(callLocation,
				$"Both parameters of callback function must have the same type, but are {callbackParameters[0].ResultTypesToString()} and {callbackParameters[1].ResultTypesToString()}");
		}

		return list.Select(e => new[] { e.ToExpressionNode() });
	}

	protected override IEnumerable<ExpressionResult> EvaluateListManipulation(TokenLocation callLocation,
		IInterpreter interpreter, IEnumerable<IExpressionNode[]> arguments, FunctionDefinition callback)
	{
		var arr = arguments.ToArray();

		Array.Sort(arr, (a, b) =>
		{
			var args = new[] { a[0], b[0] };

			var result = callback.Invoke(callLocation, interpreter, args);
			if (result is not NumberResult numberResult)
			{
				throw new InvalidResultTypeException(callLocation, result, ResultType.Number);
			}

			return Math.Sign(numberResult);
		});

		foreach (var arg in arr)
		{
			yield return arg[0].EvaluateUsing(interpreter);
		}
	}
}
