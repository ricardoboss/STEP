using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

public class IssetFunction : GenericFunction<StringResult>
{
	public const string Identifier = "isset";

	protected override IEnumerable<NativeParameter> NativeParameters { get; } =
	[
		new(OnlyString, "variableName"),
	];

	protected override BoolResult Invoke(TokenLocation callLocation, Interpreter interpreter, StringResult argument1)
	{
		return interpreter.CurrentScope.Exists(argument1.Value, includeParent: true);
	}
}
