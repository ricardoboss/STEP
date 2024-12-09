using StepLang.Expressions.Results;
using StepLang.Tokenizing;

namespace StepLang.Interpreting;

public class InvalidResultTypeException(
	TokenLocation evaluationLocation,
	ExpressionResult got,
	params ResultType[] expected)
	: IncompatibleTypesException(4, evaluationLocation, BuildMessage(got, expected), GeneralHelpText)
{
	private const string GeneralHelpText =
		"An expression evaluated to an unexpected type. Check what types of expression results are allowed in the current context.";

	private static string ToExpectation(IReadOnlyList<ResultType> expected)
	{
		return expected.Count switch
		{
			0 => "nothing",
			1 => $"a {expected[0].ToTypeName()}",
			_ => $"one of {string.Join(", ", expected.Select(e => e.ToTypeName()))}",
		};
	}

	private static string BuildMessage(ExpressionResult got, IReadOnlyList<ResultType> expected)
	{
		var expectation = ToExpectation(expected);
		var gotString = got.ResultType switch
		{
			ResultType.Void => "void",
			ResultType.Null => "null",
			ResultType.Function => "a function",
			ResultType.Str => $"a string (\"{got}\")",
			_ => $"a {got.ResultType.ToTypeName()} ({got})",
		};

		return $"Invalid result type: expected {expectation}, got {gotString}";
	}
}
