using StepLang.Expressions.Results;
using StepLang.Parsing.Nodes.Expressions;

namespace StepLang.Framework;

public abstract class GenericOneParameterFunction : GenericParameterlessFunction
{
	protected IReadOnlyList<ResultType> Argument1Types => NativeParameters.ElementAt(0).Types;
	protected ExpressionNode? Argument1Default => NativeParameters.ElementAt(0).DefaultValue;

	protected override int GetRequiredCount()
	{
		return Argument1Default is null ? 1 : 0;
	}

	protected override ExpressionNode GetDefaultExpression(int index)
	{
		return Argument1Default ?? throw new InvalidOperationException();
	}

	protected override IReadOnlyList<ResultType> GetArgumentTypes(int index)
	{
		return Argument1Types;
	}
}
