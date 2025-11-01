using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Other;

public class RandomFunction : GenericFunction
{
	public const string Identifier = "random";

	protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyNumber;

	protected override NumberResult Invoke(TokenLocation callLocation, IInterpreter interpreter)
	{
		return interpreter.NextRandom();
	}
}
