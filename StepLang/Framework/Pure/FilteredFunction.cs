using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

public class FilteredFunction : ListManipulationFunction
{
	public const string Identifier = "filtered";

	protected override IEnumerable<ExpressionResult> EvaluateListManipulation(TokenLocation callLocation,
		Interpreter interpreter, IEnumerable<ExpressionNode[]> arguments, FunctionDefinition callback)
	{
		return arguments.Where(args =>
		{
			var result = callback.Invoke(callLocation, interpreter, args);
			if (result is not BoolResult boolResult)
			{
				throw new InvalidResultTypeException(callLocation, result, ResultType.Bool);
			}

			return boolResult;
		}).Select(args => args[0].EvaluateUsing(interpreter));
	}
}
