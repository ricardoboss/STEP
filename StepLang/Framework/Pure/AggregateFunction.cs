using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing.Nodes.VariableDeclarations;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

public class AggregateFunction : GenericFunction<ExpressionResult, ListResult, FunctionResult>
{
	public const string Identifier = "aggregate";

	protected override IEnumerable<NativeParameter> NativeParameters =>
	[
		new(AnyValueType, "initial"),
		new(OnlyList, "subject"),
		new(OnlyFunction, "aggregator"),
	];

	protected override IEnumerable<ResultType> ReturnTypes => AnyValueType;

	protected override ExpressionResult Invoke(TokenLocation callLocation, IInterpreter interpreter,
		ExpressionResult argument1,
		ListResult argument2, FunctionResult argument3)
	{
		// ReSharper disable once InlineTemporaryVariable
		var initial = argument1;
		var list = argument2.DeepClone().Value;
		var aggregator = argument3.Value;

		if (aggregator.Parameters.Count != 2)
		{
			throw new InvalidArgumentTypeException(callLocation,
				$"Callback function must have 2 parameters, but has {aggregator.Parameters.Count}");
		}

		var accumulatorParameter = aggregator.Parameters[0];
		if (!accumulatorParameter.HasResultType(initial.ResultType))
		{
			throw new InvalidArgumentTypeException(callLocation,
				$"First parameter of callback function must accept initial value type {initial.ResultType}, but is {accumulatorParameter.ResultTypesToString()}");
		}

		return list.Aggregate(initial, (accumulator, element) =>
		{
			var args = new[] { accumulator.ToExpressionNode(), element.ToExpressionNode() };

			var result = aggregator.Invoke(callLocation, interpreter, args);

			return result;
		});
	}
}
