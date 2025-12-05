using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing.Nodes.Expressions;
using StepLang.Tokenizing;

namespace StepLang.Framework;

public abstract class GenericFunction : GenericParameterlessFunction
{
	protected abstract ExpressionResult Invoke(TokenLocation callLocation, IInterpreter interpreter);

	protected override IEnumerable<NativeParameter> NativeParameters { get; } = [];

	public override ExpressionResult Invoke(TokenLocation callLocation, IInterpreter interpreter,
		IReadOnlyList<ExpressionNode> arguments)
	{
		using var span = Telemetry.Profile();

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
		IReadOnlyList<ExpressionNode> arguments)
	{
		using var span = Telemetry.Profile("1 arg");

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
		IReadOnlyList<ExpressionNode> arguments)
	{
		using var span = Telemetry.Profile("2 args");

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
		IReadOnlyList<ExpressionNode> arguments)
	{
		using var span = Telemetry.Profile("3 args");

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
