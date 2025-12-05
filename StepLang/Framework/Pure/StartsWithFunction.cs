using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

public class StartsWithFunction : GenericFunction<StringResult, StringResult>
{
	public const string Identifier = "startsWith";

	protected override IEnumerable<NativeParameter> NativeParameters { get; } =
	[
		new(OnlyString, "subject"),
		new(OnlyString, "prefix"),
	];

	protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyBool;

	protected override BoolResult Invoke(TokenLocation callLocation, IInterpreter interpreter, StringResult argument1,
		StringResult argument2)
	{
		using var span = Telemetry.Profile();

		return argument1.Value.GraphemeStartsWith(argument2.Value);
	}
}
