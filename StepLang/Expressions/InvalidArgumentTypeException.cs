using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;
using System.Globalization;
using System.Text;

namespace StepLang.Expressions;

internal sealed class InvalidArgumentTypeException(TokenLocation location, string message) : IncompatibleTypesException(
	3, location, message,
	"Make sure you're passing the correct type of argument to the function.")
{
	private static string BuildMessage(IReadOnlyList<ResultType> acceptedTypes, ResultType gotType)
	{
		var sb = new StringBuilder("Invalid argument type. Expected ");

		sb.Append(acceptedTypes.Count switch
		{
			0 => "no arguments",
			1 => $"{acceptedTypes[0].ToTypeName()}",
			_ =>
				$"{string.Join(", ", acceptedTypes.Take(acceptedTypes.Count - 1).Select(t => t.ToTypeName()))} or {acceptedTypes[^1].ToTypeName()}",
		});

		sb.Append(CultureInfo.InvariantCulture, $", but got {gotType.ToTypeName()}");

		return sb.ToString();
	}

	public InvalidArgumentTypeException(TokenLocation location, IReadOnlyList<ResultType> acceptedParameterTypes,
		ExpressionResult argument) : this(location, BuildMessage(acceptedParameterTypes, argument.ResultType))
	{
	}
}
