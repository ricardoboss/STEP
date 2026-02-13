using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing.Nodes.Expressions;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

public class MaxFunction : NativeFunction
{
	public const string Identifier = "max";

	protected override IEnumerable<NativeParameter> NativeParameters { get; } =
	[
		new([ResultType.Number], "...values"),
	];

	protected override IEnumerable<ResultType> ReturnTypes { get; } = [ResultType.Number];

	public override NumberResult Invoke(TokenLocation callLocation, IInterpreter interpreter,
		IReadOnlyList<IExpressionNode> arguments)
	{
		CheckArgumentCount(callLocation, arguments, 1, int.MaxValue);

		return arguments
			.Select(argument => argument.EvaluateUsing(interpreter))
			.OfType<NumberResult>()
			.Max(argument => argument.Value);
	}
}
