using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Other;

public class FileExistsFunction : GenericFunction<StringResult>
{
	public const string Identifier = "fileExists";

	protected override IEnumerable<NativeParameter> NativeParameters { get; } =
	[
		new(OnlyString, "path"),
	];

	protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyBool;

	protected override BoolResult Invoke(TokenLocation callLocation, IInterpreter interpreter, StringResult argument1)
	{
		var path = argument1.Value;
		var info = callLocation.GetFileInfoFromPath(path);

		return info.Exists;
	}
}
