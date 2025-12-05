using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing.Nodes.Expressions;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

public class LengthFunction : NativeFunction
{
	public const string Identifier = "length";

	protected override IEnumerable<NativeParameter> NativeParameters { get; } =
	[
		new([ResultType.Str, ResultType.List, ResultType.Map], "subject"),
	];

	protected override IEnumerable<ResultType> ReturnTypes { get; } = [ResultType.Number];

	public override NumberResult Invoke(TokenLocation callLocation, IInterpreter interpreter,
		IReadOnlyList<ExpressionNode> arguments)
	{
		using var span = Telemetry.Profile();

		CheckArgumentCount(callLocation, arguments);

		var subjectResult = arguments.Single().EvaluateUsing(interpreter);

		return subjectResult switch
		{
			StringResult { Value: var str } => str.GraphemeLength(),
			ListResult { Value: var list } => list.Count,
			MapResult { Value: var map } => map.Count,
			_ => throw new InvalidArgumentTypeException(arguments.Single().Location, NativeParameters.Single().Types,
				subjectResult),
		};
	}
}
