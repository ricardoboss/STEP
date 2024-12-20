using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

public class ClampFunction : GenericFunction<NumberResult, NumberResult, NumberResult>
{
	public const string Identifier = "clamp";

	protected override IEnumerable<NativeParameter> NativeParameters { get; } =
	[
		new(OnlyNumber, "min"),
		new(OnlyNumber, "max"),
		new(OnlyNumber, "x"),
	];

	protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyNumber;

	protected override ExpressionResult Invoke(TokenLocation callLocation, Interpreter interpreter,
		NumberResult argument1, NumberResult argument2, NumberResult argument3)
	{
		var min = argument1;
		var max = argument2;
		var x = argument3;

		return new NumberResult(Math.Max(min, Math.Min(max, x)));
	}
}
