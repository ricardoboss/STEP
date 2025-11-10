using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Parsing.Nodes.Expressions;
using StepLang.Tokenizing;

namespace StepLang.Framework;

public abstract class GenericFunction : GenericParameterlessFunction
{
	protected abstract ExpressionResult Invoke(TokenLocation callLocation, IInterpreter interpreter);

	protected override IEnumerable<NativeParameter> NativeParameters { get; } = [];

	public override ExpressionResult Invoke(TokenLocation callLocation, IInterpreter interpreter,
		IReadOnlyList<IExpressionNode> arguments)
	{
		if (arguments.Count > 0)
		{
			throw new InvalidArgumentCountException(callLocation, 0, arguments.Count);
		}

		return Invoke(callLocation, interpreter);
	}
}

public abstract class GenericFunction<T1> : GenericOneParameterFunction where T1 : ExpressionResult
{
	protected abstract ExpressionResult Invoke(TokenLocation callLocation, IInterpreter interpreter, T1 argument1);

	public override ExpressionResult Invoke(TokenLocation callLocation, IInterpreter interpreter,
		IReadOnlyList<IExpressionNode> arguments)
	{
		var requiredCount = GetRequiredCount();
		var totalCount = GetArgumentTotalCount();

		if (arguments.Count < requiredCount || arguments.Count > totalCount)
		{
			throw new InvalidArgumentCountException(callLocation, requiredCount, arguments.Count);
		}

		var argument1 = GetArgument<T1>(0, interpreter, arguments);

		return Invoke(callLocation, interpreter, argument1);
	}
}

public abstract class GenericFunction<T1, T2> : GenericTwoParameterFunction
	where T1 : ExpressionResult
	where T2 : ExpressionResult
{
	protected abstract ExpressionResult Invoke(TokenLocation callLocation, IInterpreter interpreter, T1 argument1,
		T2 argument2);

	public override ExpressionResult Invoke(TokenLocation callLocation, IInterpreter interpreter,
		IReadOnlyList<IExpressionNode> arguments)
	{
		var requiredCount = GetRequiredCount();
		var totalCount = GetArgumentTotalCount();

		if (arguments.Count < requiredCount || arguments.Count > totalCount)
		{
			throw new InvalidArgumentCountException(callLocation, requiredCount, arguments.Count);
		}

		var argument1 = GetArgument<T1>(0, interpreter, arguments);
		var argument2 = GetArgument<T2>(1, interpreter, arguments);

		return Invoke(callLocation, interpreter, argument1, argument2);
	}
}

public abstract class GenericFunction<T1, T2, T3> : GenericThreeParameterFunction
	where T1 : ExpressionResult
	where T2 : ExpressionResult
	where T3 : ExpressionResult
{
	protected abstract ExpressionResult Invoke(TokenLocation callLocation, IInterpreter interpreter, T1 argument1,
		T2 argument2, T3 argument3);

	public override ExpressionResult Invoke(TokenLocation callLocation, IInterpreter interpreter,
		IReadOnlyList<IExpressionNode> arguments)
	{
		var requiredCount = GetRequiredCount();
		var totalCount = GetArgumentTotalCount();

		if (arguments.Count < requiredCount || arguments.Count > totalCount)
		{
			throw new InvalidArgumentCountException(callLocation, requiredCount, arguments.Count);
		}

		var argument1 = GetArgument<T1>(0, interpreter, arguments);
		var argument2 = GetArgument<T2>(1, interpreter, arguments);
		var argument3 = GetArgument<T3>(2, interpreter, arguments);

		return Invoke(callLocation, interpreter, argument1, argument2, argument3);
	}
}

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

	protected virtual IExpressionNode GetDefaultExpression(int index)
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
		IReadOnlyList<IExpressionNode> arguments) where TArgument : ExpressionResult
	{
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

public abstract class GenericOneParameterFunction : GenericParameterlessFunction
{
	protected IReadOnlyList<ResultType> Argument1Types => NativeParameters.ElementAt(0).Types;
	protected IExpressionNode? Argument1Default => NativeParameters.ElementAt(0).DefaultValue;

	protected override int GetRequiredCount()
	{
		return Argument1Default is null ? 1 : 0;
	}

	protected override IExpressionNode GetDefaultExpression(int index)
	{
		return Argument1Default ?? throw new InvalidOperationException();
	}

	protected override IReadOnlyList<ResultType> GetArgumentTypes(int index)
	{
		return Argument1Types;
	}
}

public abstract class GenericTwoParameterFunction : GenericOneParameterFunction
{
	protected IReadOnlyList<ResultType> Argument2Types => NativeParameters.ElementAt(1).Types;
	protected IExpressionNode? Argument2Default => NativeParameters.ElementAt(1).DefaultValue;

	protected override int GetRequiredCount()
	{
		var required = 0;

		if (Argument1Default is null)
		{
			required++;
		}

		if (Argument2Default is null)
		{
			required++;
		}

		return required;
	}

	protected override IExpressionNode GetDefaultExpression(int index)
	{
		return index switch
		{
			0 => Argument1Default ?? throw new InvalidOperationException(),
			1 => Argument2Default ?? throw new InvalidOperationException(),
			_ => throw new ArgumentOutOfRangeException(nameof(index)),
		};
	}

	protected override IReadOnlyList<ResultType> GetArgumentTypes(int index)
	{
		return index switch
		{
			0 => Argument1Types,
			1 => Argument2Types,
			_ => throw new ArgumentOutOfRangeException(nameof(index)),
		};
	}
}

public abstract class GenericThreeParameterFunction : GenericTwoParameterFunction
{
	protected IReadOnlyList<ResultType> Argument3Types => NativeParameters.ElementAt(2).Types;
	protected IExpressionNode? Argument3Default => NativeParameters.ElementAt(2).DefaultValue;

	protected override int GetRequiredCount()
	{
		var required = 0;

		if (Argument1Default is null)
		{
			required++;
		}

		if (Argument2Default is null)
		{
			required++;
		}

		if (Argument3Default is null)
		{
			required++;
		}

		return required;
	}

	protected override IExpressionNode GetDefaultExpression(int index)
	{
		return index switch
		{
			0 => Argument1Default ?? throw new InvalidOperationException(),
			1 => Argument2Default ?? throw new InvalidOperationException(),
			2 => Argument3Default ?? throw new InvalidOperationException(),
			_ => throw new ArgumentOutOfRangeException(nameof(index)),
		};
	}

	protected override IReadOnlyList<ResultType> GetArgumentTypes(int index)
	{
		return index switch
		{
			0 => Argument1Types,
			1 => Argument2Types,
			2 => Argument3Types,
			_ => throw new ArgumentOutOfRangeException(nameof(index)),
		};
	}
}
