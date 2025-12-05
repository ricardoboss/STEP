using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Parsing;
using StepLang.Parsing.Nodes.Expressions;

namespace StepLang.Framework;

public abstract class GenericParameterlessFunction : NativeFunction
{
	protected virtual int GetRequiredCount()
	{
		return 0;
	}

	protected virtual int GetArgumentTotalCount()
	{
		return NativeParameters.Count();
	}

	protected virtual ExpressionNode GetDefaultExpression(int index)
	{
		throw new InvalidOperationException();
	}

	protected virtual IReadOnlyList<ResultType> GetArgumentTypes(int index)
	{
		throw new InvalidOperationException();
	}

	private TArgument GetDefaultArgumentValue<TArgument>(int index, IExpressionEvaluator interpreter)
	{
		var defaultExpression = GetDefaultExpression(index);
		var defaultResult = defaultExpression.EvaluateUsing(interpreter);
		if (defaultResult is not TArgument defaultArgumentResult)
		{
			throw new InvalidArgumentTypeException(defaultExpression.Location, GetArgumentTypes(index), defaultResult);
		}

		return defaultArgumentResult;
	}

	protected TArgument GetArgument<TArgument>(int index, IExpressionEvaluator interpreter,
		IReadOnlyList<ExpressionNode> arguments) where TArgument : ExpressionResult
	{
		using var span = Telemetry.Profile();

		if (arguments.Count < index + 1)
		{
			return GetDefaultArgumentValue<TArgument>(index, interpreter);
		}

		var argumentResult = arguments[index].EvaluateUsing(interpreter);
		if (argumentResult is not TArgument typedResult)
		{
			throw new InvalidArgumentTypeException(arguments[index].Location, GetArgumentTypes(index), argumentResult);
		}

		return typedResult;
	}
}
