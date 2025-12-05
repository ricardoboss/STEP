using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Conversion;

public class ToBoolFunction : GenericFunction<ExpressionResult>
{
	public const string Identifier = "toBool";

	protected override IEnumerable<NativeParameter> NativeParameters { get; } =
	[
		new(AnyValueType, "value"),
	];

	protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyBool;

	protected override BoolResult Invoke(TokenLocation callLocation, IInterpreter interpreter,
		ExpressionResult argument1)
	{
		using var span = Telemetry.Profile();

		return argument1.IsTruthy();
	}
}
