using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

public class RoundFunction : GenericFunction<NumberResult>
{
	public const string Identifier = "round";

	protected override IEnumerable<NativeParameter> NativeParameters { get; } =
		[new(OnlyNumber, "x")];

	protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyNumber;

	protected override NumberResult Invoke(TokenLocation callLocation, IInterpreter interpreter, NumberResult argument1)
	{
		return Math.Round(argument1.Value);
	}
}
