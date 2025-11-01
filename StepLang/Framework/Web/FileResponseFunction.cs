using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Web;

public class FileResponseFunction : GenericFunction<StringResult, ExpressionResult>
{
	public const string Identifier = "fileResponse";

	/// <inheritdoc />
	protected override IEnumerable<NativeParameter> NativeParameters { get; } =
	[
		new(OnlyString, "file"),
		new(NullableNumber, "status", NullResult.Instance),
	];

	/// <inheritdoc />
	protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyMap;

	/// <inheritdoc />
	protected override MapResult Invoke(TokenLocation callLocation, IInterpreter interpreter, StringResult argument1,
		ExpressionResult argument2)
	{
		var path = argument1.Value;
		if (!File.Exists(path))
		{
			return new Dictionary<string, ExpressionResult> { ["status"] = new NumberResult(404) };
		}

		var fileContents = File.ReadAllText(path);

		var response = new Dictionary<string, ExpressionResult> { ["body"] = new StringResult(fileContents) };

		if (argument2 is NumberResult status)
		{
			response["status"] = status;
		}

		return response;
	}
}
