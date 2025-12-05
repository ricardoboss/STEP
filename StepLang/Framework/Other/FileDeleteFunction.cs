using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Other;

public class FileDeleteFunction : GenericFunction<StringResult>
{
	public const string Identifier = "fileDelete";

	protected override IEnumerable<NativeParameter> NativeParameters { get; } =
	[
		new(OnlyString, "path"),
	];

	protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyBool;

	protected override BoolResult Invoke(TokenLocation callLocation, IInterpreter interpreter, StringResult argument1)
	{
		using var span = Telemetry.Profile();

		var path = argument1.Value;
		var info = callLocation.GetFileInfoFromPath(path);

		try
		{
			File.Delete(info.FullName);
		}
		catch (IOException)
		{
			return false;
		}

		return true;
	}
}
