using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Other;

public class TimestampFunction : GenericFunction
{
	public const string Identifier = "timestamp";

	protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyNumber;

	protected override NumberResult Invoke(TokenLocation callLocation, IInterpreter interpreter)
	{
		using var span = Telemetry.Profile();

		return interpreter.Time.GetUtcNow().ToUnixTimeMilliseconds();
	}
}
