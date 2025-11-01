using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

public class InterpolateFunction : GenericFunction<NumberResult, NumberResult, NumberResult>
{
	public const string Identifier = "interpolate";

	protected override IEnumerable<NativeParameter> NativeParameters { get; } =
	[
		new(OnlyNumber, "a"),
		new(OnlyNumber, "b"),
		new(OnlyNumber, "t"),
	];

	protected override ExpressionResult Invoke(TokenLocation callLocation, IInterpreter interpreter,
		NumberResult argument1, NumberResult argument2, NumberResult argument3)
	{
		var a = argument1;
		var b = argument2;
		var t = argument3;

		return a + (b - a) * t;
	}
}
