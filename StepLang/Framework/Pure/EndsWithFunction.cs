using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

public class EndsWithFunction : GenericFunction<StringResult, StringResult>
{
	public const string Identifier = "endsWith";

	protected override IEnumerable<NativeParameter> NativeParameters { get; } =
	[
		new(OnlyString, "subject"),
		new(OnlyString, "suffix"),
	];

	protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyBool;

	protected override BoolResult Invoke(TokenLocation callLocation, Interpreter interpreter, StringResult argument1,
		StringResult argument2)
	{
		return argument1.Value.GraphemeEndsWith(argument2.Value);
	}
}
