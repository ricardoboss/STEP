using System.Diagnostics.CodeAnalysis;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Other;

public class RandomFunction : GenericFunction
{
	public const string Identifier = "random";

	protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyNumber;

	[SuppressMessage("Security", "CA5394:Do not use insecure randomness",
		Justification = "This is not for security purposes")]
	protected override NumberResult Invoke(TokenLocation callLocation, Interpreter interpreter)
	{
		return interpreter.Random.NextDouble();
	}
}
