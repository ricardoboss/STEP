using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

public class CloneFunction : GenericFunction<ExpressionResult>
{
	public const string Identifier = "clone";

	protected override IEnumerable<NativeParameter> NativeParameters { get; } =
	[
		new(AnyValueType, "subject"),
	];

	protected override IEnumerable<ResultType> ReturnTypes { get; } = AnyValueType;

	protected override ExpressionResult Invoke(TokenLocation callLocation, IInterpreter interpreter,
		ExpressionResult argument1)
	{
		using var span = Telemetry.Profile();

		return argument1.DeepClone();
	}
}
