using StepLang.Expressions.Results;
using StepLang.Parsing.Nodes.Expressions;

namespace StepLang.Framework;

public abstract class GenericTwoParameterFunction : GenericOneParameterFunction
{
	protected IReadOnlyList<ResultType> Argument2Types => NativeParameters.ElementAt(1).Types;
	protected ExpressionNode? Argument2Default => NativeParameters.ElementAt(1).DefaultValue;

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

	protected override ExpressionNode GetDefaultExpression(int index)
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
