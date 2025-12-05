using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

public class ReadFunction : GenericFunction
{
	public const string Identifier = "read";

	protected override IEnumerable<ResultType> ReturnTypes { get; } = NullableString;

	protected override ExpressionResult Invoke(TokenLocation callLocation, IInterpreter interpreter)
	{
		using var span = Telemetry.Profile();

		if (interpreter.StdIn is not { } stdIn)
		{
			return NullResult.Instance;
		}

		var character = stdIn.Read();
		if (character < 0)
		{
			return NullResult.Instance;
		}

		return new StringResult(char.ConvertFromUtf32(character));
	}
}
