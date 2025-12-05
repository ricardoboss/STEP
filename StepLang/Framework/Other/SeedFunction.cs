using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Other;

public class SeedFunction : GenericFunction<ExpressionResult>
{
	public const string Identifier = "seed";

	protected override IEnumerable<NativeParameter> NativeParameters { get; } =
	[
		new(NullableNumber, "seed"),
	];

	protected override ExpressionResult Invoke(TokenLocation callLocation, IInterpreter interpreter,
		ExpressionResult argument1)
	{
		using var span = Telemetry.Profile();

		int seed;
		if (argument1 is NumberResult numberResult)
		{
			seed = numberResult;
		}
		else
		{
			seed = (int)DateTime.Now.Ticks;
		}

		interpreter.SetRandomSeed(seed);

		return VoidResult.Instance;
	}
}
