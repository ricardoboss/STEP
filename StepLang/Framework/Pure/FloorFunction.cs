using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

public class FloorFunction : GenericFunction<NumberResult>
{
	public const string Identifier = "floor";

	protected override IEnumerable<NativeParameter> NativeParameters { get; } =
		[new(OnlyNumber, "x")];

	protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyNumber;

	protected override NumberResult Invoke(TokenLocation callLocation, IInterpreter interpreter, NumberResult argument1)
	{
		return Math.Floor(argument1.Value);
	}
}
