using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing.Nodes.Expressions;
using StepLang.Parsing.Nodes.VariableDeclarations;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

public abstract class ListManipulationFunction : GenericFunction<ListResult, FunctionResult>
{
	protected override IEnumerable<NativeParameter> NativeParameters { get; } =
	[
		new(OnlyList, "subject"),
		new(OnlyFunction, "callback"),
	];

	protected override ExpressionResult Invoke(TokenLocation callLocation, IInterpreter interpreter,
		ListResult argument1, FunctionResult argument2)
	{
		var list = argument1.DeepClone().Value;
		var callback = argument2.Value;

		var args = PrepareArgsForCallback(callLocation, list, callback);
		var result = EvaluateListManipulation(callLocation, interpreter, args, callback).ToList();

		return new ListResult(result);
	}

	protected virtual IEnumerable<IExpressionNode[]> PrepareArgsForCallback(TokenLocation callLocation,
		IEnumerable<ExpressionResult> list, FunctionDefinition callback)
	{
		var callbackParameters = callback.Parameters.ToList();
		Func<ExpressionResult, int, IExpressionNode[]> argsConverter;

		switch (callbackParameters.Count)
		{
			case < 1 or > 2:
				throw new InvalidArgumentTypeException(callLocation,
					$"Callback function must have 1 or 2 parameters, but has {callbackParameters.Count}");
			case 2:
				if (!callbackParameters[1].HasResultType(ResultType.Number))
				{
					throw new InvalidArgumentTypeException(callLocation,
						$"Second parameter of callback function must accept numbers, but is {callbackParameters[1].ResultTypesToString()}");
				}

				argsConverter = (element, index) =>
				{
					var elementExpression = element.ToExpressionNode();
					var indexExpression = (LiteralExpressionNode)index;

					return [elementExpression, indexExpression];
				};

				break;
			default:
				argsConverter = (element, _) => { return [element.ToExpressionNode()]; };

				break;
		}

		return list.Select(argsConverter);
	}

	protected abstract IEnumerable<ExpressionResult> EvaluateListManipulation(TokenLocation callLocation,
		IInterpreter interpreter, IEnumerable<IExpressionNode[]> arguments, FunctionDefinition callback);
}
