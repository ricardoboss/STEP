using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing.Nodes.Expressions;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

public class ConvertedFunction : ListManipulationFunction
{
	public const string Identifier = "converted";

	protected override IEnumerable<ExpressionResult> EvaluateListManipulation(TokenLocation callLocation,
		IInterpreter interpreter, IEnumerable<ExpressionNode[]> arguments, FunctionDefinition callback)
	{
		return arguments.Select(args => callback.Invoke(callLocation, interpreter, args));
	}
}
